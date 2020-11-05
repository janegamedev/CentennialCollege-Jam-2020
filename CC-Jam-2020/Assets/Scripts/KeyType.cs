using Scriptables;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/KeyType")]
public class KeyType : ObjectType
{
    public BoolVariable hasKey;
    
    public override void Interact(ObjectInstance inst)
    { 
        hasKey.SetValue(true);
        inst.DestroyObject();
    }
}