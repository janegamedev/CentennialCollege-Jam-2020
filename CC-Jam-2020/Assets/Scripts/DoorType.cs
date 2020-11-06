using Scriptables;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/DoorType")]
public class DoorType : ObjectType
{
    public BoolVariable hasKey, hasSoul;
    public GameEvent roomCompleteEvent;
    
    public override void Interact(ObjectInstance inst)
    { 
        if(!hasKey || !hasSoul) return;
        
        hasKey.SetValue(false);
        Debug.Log("Here");
        roomCompleteEvent.Raise();
        
        base.Interact(inst);
    }
}