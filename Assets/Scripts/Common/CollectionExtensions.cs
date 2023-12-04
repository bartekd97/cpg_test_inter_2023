using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public static class CollectionExtensions
    {
        public static T GetRandom<T>(this IReadOnlyList<T> list)
        {
            if (list.Count == 0)
                return default(T);

            var i = Random.Range(0, list.Count);
            return list[i];
        }
    }
}
