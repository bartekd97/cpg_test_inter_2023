using UnityEngine;

namespace Common
{
    public static class UnityExtensions
    {
        public static T Instantiate<T>(this GameObject prefab, Transform parent = null) where T : Component
        {
            return GameObject.Instantiate(prefab, parent).GetComponent<T>();
        }
    }
}
