using UnityEngine;

namespace Common
{
    public static class UnityExtensions
    {
        public static T Instantiate<T>(this GameObject prefab, Transform parent = null) where T : Component
        {
            return GameObject.Instantiate(prefab, parent).GetComponent<T>();
        }

        public static Vector2 Clamp(this Rect rect, Vector2 point)
        {
            return new(
                Mathf.Clamp(point.x, rect.xMin, rect.xMax),
                Mathf.Clamp(point.y, rect.yMin, rect.yMax)
            );
        }
    }
}
