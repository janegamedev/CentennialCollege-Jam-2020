using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid")]
public class Grid : ScriptableObject
{
    public Texture2D map;
    [TableList] public ColorTile[] tiles;
    public int size;
    public bool IsTileEmpty(int x, int y) => GetColor(x, y) == Color.white;
    public Tile GetTile(int x, int y) => tiles.First(t => t.color == GetColor(x, y)).tile;
    private Color GetColor(int x, int y) => map.GetPixel(x, y);
}

[System.Serializable]
public class ColorTile
{
    public Color color;
    public Tile tile;
}