using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Variables/Vector2Int")]
    public class Vector2IntVariable : ScriptableObject
    {
        public Vector2Int value;

        public void SetValue(Vector2Int v) => value = v;
    }
}