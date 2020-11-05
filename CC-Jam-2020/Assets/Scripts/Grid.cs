using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid")]
public class Grid : SerializedScriptableObject
{
    [TableList] public ColorTile[] tiles = new ColorTile[5]
    {
        new ColorTile(Color.black), 
        new ColorTile(Color.gray), 
        new ColorTile(Color.yellow), 
        new ColorTile(Color.magenta), 
        new ColorTile(Color.blue), 
    };
    
    private static int maxObjects = 6;

    public Object GetObject(int x, int y) => tiles[cells[x, y] - 1].obj;
    public bool IsCellEmpty(int x, int y) => cells[x, y] == 0;
    public int size => cells.GetLength(0);
    public Vector2Int CharacterSpawn;
    public Object character;
    
    [TableMatrix(DrawElementMethod = "DrawTile", SquareCells = true)]
    public int[,] cells;

    private static int DrawTile(Rect rect, int index)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            if (Event.current.button == 0)
            {
                index ++;
                if (index > maxObjects)
                    index = 0;
            }
            else if(Event.current.button == 1)
            {
                index = 0;
            }

            GUI.changed = true;
            Event.current.Use();
        }

        Color c = Color.black;
        switch (index)
        {
            case 0: // empty
                c = Color.white;
                break;
            case 1: // wall
                c = Color.black;
                break;
            case 2: // box
                c = Color.gray;
                break;
            case 3: // player
                c = Color.yellow;
                break;
            case 4: // key
                c = Color.magenta;
                break;
            case 5: // door
                c = Color.blue;
                break;
            case 6: //spike
                c= Color.red;
                break;
        }
        
        EditorGUI.DrawRect(rect.Padding(1), c);

        return index;
    }
}

[System.Serializable]
public class ColorTile
{
    public Color color;
    public Object obj;

    public ColorTile(Color c)
    {
        color = c;
    }
}