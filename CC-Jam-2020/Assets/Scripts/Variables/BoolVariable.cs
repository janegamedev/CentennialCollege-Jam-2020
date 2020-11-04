using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(menuName = "Variables/Bool")]
    public class BoolVariable : ScriptableObject
    {
        public bool value;

        public void SetValue(bool v) => value = v;
    }
}