using Scriptables;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/DamageObjectType")]
public class DamageObjectType : ObjectType
{
    public BoolVariable hasKey;
    public GameEvent deathEvent;
    
    public override void Interact(ObjectInstance inst)
    {
        hasKey.SetValue(false);
        deathEvent.Raise();
    }
}