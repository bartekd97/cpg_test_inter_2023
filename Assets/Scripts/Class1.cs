using Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Class1 : MonoBehaviour
    {
        public LevelConfigProvider configProvider;
        public LevelConfig config;

        private void Start()
        {
            config = configProvider.Provide();
            //var json = JsonUtility.ToJson(config, true);
            //File.WriteAllText("test.json", json);
        }
    }
}
