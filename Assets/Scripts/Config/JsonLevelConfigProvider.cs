using System;
using System.IO;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu]
    public class JsonLevelConfigProvider : LevelConfigProvider
    {
        public enum Source
        {
            PlainText,
            StreamingAssetsFile
        }

        public Source source;
        public string value;

        public override LevelConfig Provide()
        {
            var json = GetJSON();
            var config = new LevelConfig();
            JsonUtility.FromJsonOverwrite(json, config);
            return config;
        }

        string GetJSON()
        {
            switch (source)
            {
                case Source.PlainText:
                    {
                        return value;
                    }

                case Source.StreamingAssetsFile:
                    {
                        var path = Path.Combine(Application.streamingAssetsPath, value);
                        var json = File.ReadAllText(path);
                        return json;
                    }
            }

            throw new NotImplementedException();
        }
    }
}
