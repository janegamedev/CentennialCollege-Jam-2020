using UnityEngine;

[CreateAssetMenu(menuName = "Object")]
public class Object : ScriptableObject
{
    public string tileName;
    public GameObject prefab;
    public ObjectType type;
    public bool requireRotation, isMovable, isPassable, isCharacter;
}