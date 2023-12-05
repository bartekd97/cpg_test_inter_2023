using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

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

        public async override Task<LevelConfig> Provide()
        {
            var json = await GetJSON();
            var config = new LevelConfig();
            JsonUtility.FromJsonOverwrite(json, config);
            return config;
        }

        async Task<string> GetJSON()
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
#if UNITY_ANDROID
                        var request = UnityWebRequest.Get(path);
                        request.SendWebRequest();

                        while (!request.isDone)
                            await Task.Delay(1);

                        var json = request.downloadHandler.text;
#else
                        var json = File.ReadAllText(path);
#endif
                        return json;
                    }
            }

            throw new NotImplementedException();
        }
    }
}
