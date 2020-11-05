using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Variables/GridManager")]
    public class GridManagerVariable : ScriptableObject
    {
        public GridManager value;

        public void SetValue(GridManager v) => value = v;
    }
}