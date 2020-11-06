using System.Collections.Generic;
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
    
    private static int maxObjects = 14;

    public Object GetObject(int x, int y) => tiles[cells[x, y] - 1].obj;
    public bool IsCellEmpty(int x, int y) => cells[x, y] == 0;
    public int size => cells.GetLength(0);
    public Object character, soul;

    public Vector2Int CharacterSpawn()
    {
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                if(cells[x,y] == 3)
                    return new Vector2Int(x,y);
            }
        }
        
        return Vector2Int.zero;
    }
    public Vector2Int[] SoulSpawnPos()
    {
        List<Vector2Int> pos = new List<Vector2Int>();

        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                if(cells[x,y] == 14)
                    pos.Add(new Vector2Int(x,y));
            }
        }
        
        return pos.ToArray();
    }
    
    [TableMatrix(DrawElementMethod = "DrawTile", SquareCells = true)]
    public int[,] cells;

    public AudioClip music;

    public Vector2Int KeyPosition()
    {
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                if(cells[x,y] == 4)
                    return new Vector2Int(x,y);
            }
        }

        return Vector2Int.zero;
    }


#if UNITY_EDITOR
    

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
            case 5: //spike down
                c= Color.red;
                break;
            case 6: //spike right
                c = new Color(.6320754f,0,0,1 );
                break;
            case 7: //spike up
                c = new Color(.3584906f,0,0,1 );
                break;
            case 8: //spike left
                c = new Color(1,.259434f,0.259434f,1 );
                break;
            case 9: // door down
                c = Color.blue;
                break;
            case 10: // door right
                c = new Color(0, 0, 0.735849f, 1);
                break;
            case 11: // door up
                c = new Color(0, 0, 0.3113208f, 1);
                break;
            case 12: // door left
                c = new Color(0, 0.3886709f, 1, 1);
                break;
            case 13: // portal
                c = new Color(0, 0.2735849f, 0, 1);
                break;
            case 14: // soul
                c = Color.green;
                break;
        }
        
        EditorGUI.DrawRect(rect.Padding(1), c);

        return index;
    }
    
#endif
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