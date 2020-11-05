using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Variables/ObjectInst")]
    public class ObjectInstVariable : ScriptableObject
    {
        public ObjectInstance value;

        public void SetValue(ObjectInstance v) => value = v;
    }
}