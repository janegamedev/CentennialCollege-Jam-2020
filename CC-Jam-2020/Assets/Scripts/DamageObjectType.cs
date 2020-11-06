using Scriptables;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/DamageObjectType")]
public class DamageObjectType : ObjectType
{
    public GameEvent deathEvent;
    
    public override void Interact(ObjectInstance inst)
    {
        deathEvent.Raise();
    }
}