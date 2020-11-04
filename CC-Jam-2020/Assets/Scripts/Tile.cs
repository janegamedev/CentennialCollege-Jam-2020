using UnityEngine;

[CreateAssetMenu(menuName = "Tile")]
public class Tile : ScriptableObject
{
    public string tileName;
    public GameObject prefab;
    public TileType type;
    public bool requireRotation, isStatic,isCharacter;
}