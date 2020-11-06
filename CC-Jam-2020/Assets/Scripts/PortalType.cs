using Scriptables;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/PortalType")]
public class PortalType : ObjectType
{
    public GameEvent onPortalUse;
    
    
    public override void Interact(ObjectInstance inst)
    {
        onPortalUse.Raise();
        
        base.Interact(inst);
    }
}