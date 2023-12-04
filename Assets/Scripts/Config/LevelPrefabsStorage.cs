using UnityEngine;

namespace Config
{
    [CreateAssetMenu]
    public class LevelPrefabsStorage : ScriptableObject
    {
        public GameObject cell;
        public GameObject spawner;
        public GameObject ball;
    }
}
