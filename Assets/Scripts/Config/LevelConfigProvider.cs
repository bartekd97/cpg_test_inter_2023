using UnityEngine;

namespace Config
{
    public abstract class LevelConfigProvider : ScriptableObject
    {
        public abstract LevelConfig Provide();
    }
}
