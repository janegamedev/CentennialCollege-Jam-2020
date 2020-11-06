using Scriptables;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/SoulType")]
public class SoulType : ObjectType
{
    public BoolVariable hasSoul;
    public GameEvent onSoulUpdated;
    
    public override void Interact(ObjectInstance inst)
    { 
        hasSoul.SetValue(true);
        inst.DestroyObject();
        onSoulUpdated.Raise();
        
        base.Interact(inst);
    }
}