using Scriptables;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/KeyType")]
public class KeyType : ObjectType
{
    public BoolVariable hasKey;
    public GameEvent onKeyUpdated;
  
    
    public override void Interact(ObjectInstance inst)
    { 
        hasKey.SetValue(true);
        inst.DestroyObject();
        onKeyUpdated.Raise();
        
        base.Interact(inst);
    }
}