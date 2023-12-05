using System.Threading.Tasks;
using UnityEngine;

namespace Config
{
    public abstract class LevelConfigProvider : ScriptableObject
    {
        public abstract Task<LevelConfig> Provide();
    }
}
