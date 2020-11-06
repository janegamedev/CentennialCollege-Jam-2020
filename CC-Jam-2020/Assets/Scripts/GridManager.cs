using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GridManager : MonoBehaviour
{
    [BoxGroup("SETTINGS")] public GridManager nextRoom, underworldRoom;
    [BoxGroup("SETTINGS")] public Grid setup;
    [BoxGroup("SETTINGS")] public Vector2IntVariable gravity;
    [BoxGroup("SETTINGS")] public Vector3 playerInitRotation;
    [BoxGroup("ROTATION")] public Vector2Int eRotation, qRotation;
    
    [BoxGroup("TRANSFORMS")]
    [TableList] public TileParent[] parents;
    
    [BoxGroup("VARIABLES")] public BoolVariable hasKey;
   
    private Tile[,] _grid;
    private List<ObjectInstance> _rotationRequired = new List<ObjectInstance>();
    private List<ObjectInstance> _physicsApply = new List<ObjectInstance>();
    [HideInInspector] public ObjectInstance character;

    private float _offset;
    private bool _isInit;
    private ObjectInstance _key;
    public List<ObjectInstance> RotationRequired => _rotationRequired;
   
    
    private void Unassign(ObjectInstance obj)
    {
        _grid.Value(obj.gridPos).RemoveObject(obj);
    } 

    private void Assign(ObjectInstance obj, Vector2Int pos)
    {
        _grid.Value(pos).AddObject(obj);
    }
    
    private void Awake()
    {
        CreateRoom();
        _isInit = true;
        SimulatePhysics();
    }

    public void CreateRoom()
    {
        _grid = new Tile[setup.size, setup.size];
        _offset = setup.size / 2f - .5f;

        for (int y = 0; y < _grid.GetLength(0); y++)
        {
            for (int x = 0; x < _grid.GetLength(1); x++)
            {
                Vector2Int pos = new Vector2Int(x,y);
                _grid[x,y] = new Tile(pos);
                
                if(setup.IsCellEmpty(x, y)) continue;

                Vector3 worldPosition = WorldPosition(pos);
         
                Object obj = setup.GetObject(x, y);
                
                if (obj.isCharacter || obj.isSoul) continue;

                ObjectInstance inst = Instantiate(obj.prefab, worldPosition, obj.prefab.transform.rotation, parents.First(p => p.type == obj.type).parent).GetComponent<ObjectInstance>();
                inst.SetObject(obj);
                inst.name = $"{obj.tileName}";
                
                Assign(inst, pos);

                if(obj.requireRotation)
                    _rotationRequired.Add(inst);
                if(obj.isMovable)
                    _physicsApply.Add(inst);
                if (obj.isKey)
                    _key = inst;
            }
        }

        hasKey.SetValue(false);
    }

    public ObjectInstance SpawnCharacter()
    {
        Vector2Int pos = setup.CharacterSpawn();
        
        // convert to current grid position 
        
        Vector3 worldPosition = WorldPosition(pos);

        Object obj = setup.character;

        ObjectInstance inst = Instantiate(obj.prefab, worldPosition, obj.prefab.transform.rotation, parents.First(p => p.type == obj.type).parent).GetComponent<ObjectInstance>();
        inst.SetObject(obj);
        inst.name = $"{obj.tileName}";
                
        Assign(inst, pos);
        character = inst;
        _physicsApply.Add(character);
        _rotationRequired.Add(character);
        SimulatePhysics();

        return character;
    }

    public void RemoveCharacter()
    {
        _physicsApply.Remove(character);
        _rotationRequired.Remove(character);
        _key.DestroyObject();
        _key = null;
        Unassign(character);
        character = null;
    }

    public void PlaceSoul()
    {
        Vector2Int pos = character.gridPos;
        Vector2Int[] soulSpawn = setup.SoulSpawnPos();

        float distance = 0;
        int maxIndex = 0;
        for (int i = 0; i < soulSpawn.Length; i++)
        {
            float dis = (soulSpawn[i] - pos).magnitude;
            if(dis < distance) continue;

            distance = dis;
            maxIndex = i;
        }

        Vector2Int spawnPosition = soulSpawn[maxIndex];
        Vector3 worldPosition = WorldPosition(spawnPosition);

        Object obj = setup.soul;

        ObjectInstance inst = Instantiate(obj.prefab, worldPosition, obj.prefab.transform.rotation, parents.First(p => p.type == obj.type).parent).GetComponent<ObjectInstance>();
        inst.SetObject(obj);
        inst.name = $"{obj.tileName}";
                
        Assign(inst, spawnPosition);
    }

    public void AddCharacter(ObjectInstance c)
    {
        Vector2Int pos = setup.CharacterSpawn();
        
        /*float rotation = (playerInitRotation.z - transform.eulerAngles.z) / 90;

        if (rotation != 0)
        {
            pos = pos - new Vector2Int(7, 7);
        
            for (int i = 0; i < rotation; i++)
            {
                pos = new Vector2Int(-pos.y, pos.x);
            }
        }*/
        
        Vector3 worldPosition = WorldPosition(pos);
        
        Assign(c, pos);
        character = c;
        _physicsApply.Add(character);
        _rotationRequired.Add(character);
        character.transform.SetParent(parents.First(x => x.type == character.data.type).parent);
        character.SetRotation(playerInitRotation ,0.5f);
        character.Move(worldPosition);
        SimulatePhysics();
    }
    
    /*public void PlaceKey()
    {
        Vector2Int pos = setup.KeyPosition();
        Vector3 worldPosition = WorldPosition(pos);
     
        Object obj = setup.GetObject(pos.x, pos.y);

        ObjectInstance inst = Instantiate(obj.prefab, worldPosition, obj.prefab.transform.rotation, parents.First(p => p.type == obj.type).parent).GetComponent<ObjectInstance>();
        inst.SetObject(obj);
        inst.name = $"{obj.tileName}";
        
        Assign(inst, pos);
        _key = inst;
        SimulatePhysics();
    }*/

    public void SimulatePhysics()
    {
        if(_physicsApply.Count < 1 || !_isInit) return;
        List<ObjectInstance> objects;

        if(gravity.value == Vector2Int.up)
            objects = _physicsApply.OrderByDescending(x=> x.gridPos.y).ToList();
        else
            objects = _physicsApply.OrderBy(x=> x.gridPos.y).ToList();

        foreach (ObjectInstance obj in objects)
        {
            if (!_grid.Value(obj.gridPos + gravity.value).IsPassable()) continue;
            
            while (true)
            {
                Tile nextTile = _grid.Value(obj.gridPos + gravity.value);
                if (nextTile.IsPassable())
                {
                    Unassign(obj);
                    Assign(obj, obj.gridPos + gravity.value);
                    
                }
                else
                    break;
            }
            
            obj.Move(WorldPosition(obj.gridPos));
        }
    }

    public bool TryMoveObject(ObjectInstance obj, Vector2Int dir)
    {
        Vector2Int nextPos = obj.gridPos + dir;
        Tile tile = _grid.Value(nextPos);

        if (tile.IsEmpty)
        {
            MoveObject(obj, nextPos);
            return true;
        }

        if (tile.objects.Count == 1)
        {
            ObjectInstance nextObj = tile.objects[0];
            
            if (nextObj.data.isPassable)
            {
                MoveObject(obj, nextPos);
                return true; 
            }

            if (nextObj.data.isMovable && TryMoveObject(nextObj, dir))
            {
                MoveObject(obj, nextPos);
                return true;
            }
            return false;
        }

        if (!IsTileMovable(nextPos)) return false;

        ObjectInstance movable = tile.GetMovable;
        
        if (TryMoveObject(movable, dir))
        {
            MoveObject(movable, nextPos + dir);
            return true;
        }
       
        return false;
    }

    public void CheckForPlayer()
    {
        List<ObjectInstance> sameTile = _grid.Value(character.gridPos).objects;

        foreach (ObjectInstance instance in sameTile)
        {
            instance.data.type.Interact(instance);
        }
    }

    private void MoveObject(ObjectInstance obj, Vector2Int next)
    {
        Unassign(obj);
        Assign(obj, next);
        obj.Move(WorldPosition(obj.gridPos));
    }

    public void RotateGrid(int dir)
    {
        Tile[,] temp = new Tile[_grid.GetLength(0),_grid.GetLength(1)];

        for (int y = 0; y < temp.GetLength(0); y++)
        {
            for (int x = 0; x < temp.GetLength(1); x++)
            {
                Vector2Int pos = new Vector2Int(x,y);
                temp[x,y] = new Tile(pos);
            }
        }

        // Create a complex number with input -1/1 as imaginary number
        Complex m = new Complex(0, -dir);
        
        for (int y = 0; y < temp.GetLength(0); y++)
        {
            for (int x = 0; x < temp.GetLength(1); x++)
            {
                Vector2Int pos = new Vector2Int(x,y);

                if(_grid.Value(pos).IsEmpty) continue;

                // Create a complex number with tile position
                Complex v = new Complex(x, y);
                
                // Multiplying complex pos to complex rotation
                v *= m;
                
                // Converting the result to tile position Vector 2int
                Vector2Int newPosition = new Vector2Int((int)v.Real, (int)v.Imaginary);

                Vector2Int direction = dir == 1 ? Vector2Int.up : Vector2Int.right;
                newPosition += direction * (temp.GetLength(0) - 1);

                foreach (ObjectInstance o in _grid.Value(pos).objects)
                {
                    temp[newPosition.x, newPosition.y].AddObject(o);
                }
            }
        }
        
        _grid = temp;
    }
    
    private bool IsTileEmpty(Vector2Int pos) => _grid.Value(pos).IsEmpty;
    
    private bool IsTileMovable(Vector2Int pos)
    {
        return _grid.Value(pos).objects.Any(x => x.data.isMovable && !x.data.isPassable);
    }

    public void ResetRoom()
    {
        foreach (Tile tile in _grid)
        {
            foreach (ObjectInstance o in tile.objects)
            {
                Destroy(o.gameObject);
            }
        }
        
        _physicsApply.Clear();
        _rotationRequired.Clear();
        
        transform.localRotation = Quaternion.identity;
        CreateRoom();
    }
    
    #region Calculations

    private Vector3 WorldPosition(Vector2Int pos) => new Vector3(transform.position.x - _offset + pos.x, transform.position.y + _offset - pos.y, 0);

    #endregion
}