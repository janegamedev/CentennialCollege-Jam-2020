using UnityEngine;

namespace Utilities
{
    public static class Extensions
    {
        public static T Value<T>(this T[,] array, Vector2Int pos)
        {
            return array[pos.x, pos.y];
        }
    }
}