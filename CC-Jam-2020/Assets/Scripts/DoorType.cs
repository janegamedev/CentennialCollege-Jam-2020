using Scriptables;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/DoorType")]
public class DoorType : ObjectType
{
    public BoolVariable hasKey;
    public GameEvent roomCompleteEvent;
    
    public override void Interact(ObjectInstance inst)
    { 
        if(!hasKey) return;
        
        hasKey.SetValue(false);
        roomCompleteEvent.Raise();
    }
}