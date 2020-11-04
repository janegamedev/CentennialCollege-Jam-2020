using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Sirenix.OdinInspector;
using UnityEngine;
using Variables;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GridManager : MonoBehaviour
{
    public Grid setup;
    [TableList] public TileParent[] parents;
    public BoolVariable worldInRotation;

    [TableMatrix][ShowInInspector]
    public TileInstance[,] _grid;
    private List<TileInstance> _tilesToRotate = new List<TileInstance>();
    private List<TileInstance> _tilesWithPhysics = new List<TileInstance>();
    private TileInstance _character;
    private float _offset;
    private int _half;
    private bool _isInit;
    public List<TileInstance> TilesToRotate => _tilesToRotate;

    private void Awake()
    {
        _grid = new TileInstance[setup.size, setup.size];
        _offset = setup.size / 2f - .5f;
        _half = (int) _offset;
        
        for (int y = 0; y < _grid.GetLength(0); y++)
        {
            for (int x = 0; x < _grid.GetLength(1); x++)
            {
                if(setup.IsTileEmpty(x, y)) continue;
                
                Vector3 pos = GridToWorld(new Vector2Int(x,y));
                Tile t = setup.GetTile(x, y);

                TileInstance inst = Instantiate(t.prefab, pos, Quaternion.identity, parents.First(p => p.type == t.type).parent).GetComponent<TileInstance>();
                inst.SetTile(this, t, new Vector2Int(x,y));
                inst.name = $"{t.tileName} - {x}:{y}";
                _grid[x,y] = inst;
                
                if(t.requireRotation)
                    _tilesToRotate.Add(_grid[x,y]);
                if(!t.isStatic)
                    _tilesWithPhysics.Add(_grid[x,y]);
                if (t.isCharacter)
                    _character = inst;
            }
        }

        _isInit = true;
        SimulatePhysics();
    }

    public void SimulatePhysics()
    {
        if(_tilesWithPhysics.Count < 1 || !_isInit) return;

        List<TileInstance> sort = _tilesWithPhysics.OrderByDescending(x=>x.gridPos.y).ToList();
        
        foreach (TileInstance t in sort)
        {
            if(HasTileBelow(t)) continue;
            
            MoveTileDown(t);
        }
    }

    public void MoveCharacter(Vector2Int dir)
    {
        if(!IsTileAvailable(_character.gridPos + dir)) return;

        worldInRotation.SetValue(true);
        Vector2Int pos = _character.gridPos + dir;
        _grid[_character.gridPos.x, _character.gridPos.y] = null;
        _grid[pos.x, pos.y] = _character;
        _character.gridPos = pos;
        _character.Move(GridToWorld(pos));
        Invoke(nameof(OnMoveEnd), .5f);
    }

    private void OnMoveEnd()
    {
        worldInRotation.SetValue(false);
        SimulatePhysics();
    }

    public void RotateGrid(int dir)
    {
        TileInstance[,] temp = new TileInstance[_grid.GetLength(0),_grid.GetLength(1)];

        // Create a complex number with input -1/1 as imaginary number
        Complex m = new Complex(0, -dir);
        
        for (int y = 0; y < temp.GetLength(0); y++)
        {
            for (int x = 0; x < temp.GetLength(1); x++)
            {
                if(_grid[x,y] == null) continue;
                
                //TileInstance t = _grid[x, y];
                //Vector2Int p = new Vector2Int(x /*- _half*/, y /*+ _half*/);
                
                // Create a complex number with tile position
                Complex v = new Complex(x, y);
                
                // Multiplying complex pos to complex rotation
                v *= m;
                
                // Converting the result to tile position Vector 2int
                Vector2Int pos = new Vector2Int((int)v.Real, (int)v.Imaginary);

                if (dir == 1)
                {
                    pos += Vector2Int.up * (temp.GetLength(0) - 1);
                } 
                else
                {
                    pos += Vector2Int.right * (temp.GetLength(1) - 1);
                }
                
                temp[pos.x, pos.y] = _grid[x, y];
               
                _grid[x, y].gridPos = pos;
            }
        }
        
        _grid = temp;
    }

    public bool IsTileAvailable(Vector2Int pos)
    {
        return _grid[pos.x, pos.y] == null;
        /*if (_grid[pos.x, pos.y] == null) return true;

        return !_grid[pos.x, pos.y].data.isStatic;*/
    }
    
    public bool HasTileBelow(TileInstance t) => _grid[t.gridPos.x, t.gridPos.y + 1] != null;

    public void MoveTileDown(TileInstance t)
    {
        while (true)
        {
            if (_grid[t.gridPos.x, t.gridPos.y + 1] == null)
            {
                _grid[t.gridPos.x, t.gridPos.y] = null;
                t.gridPos = new Vector2Int(t.gridPos.x, t.gridPos.y + 1);
            }
            else
                break;
        }

        _grid[t.gridPos.x, t.gridPos.y] = t;
        t.Move(GridToWorld(t.gridPos));
    }

    #region Calculations

    private Vector3 GridToWorld(Vector2Int pos) => new Vector3(transform.position.x - _offset + pos.x, transform.position.y + _offset - pos.y, 0);

    #endregion
}

public class GameManager : MonoBehaviour
{
    
    public GridManager gridManager;
    
}