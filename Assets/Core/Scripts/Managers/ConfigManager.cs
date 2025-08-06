using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace PrSuperSoldier
{
    public class ConfigManager : Manager
    {
        public string DirectoryPath = "Config";

        private Dictionary<string, IConfigData> _configDataTable = new();

        public TConfigData Get<TConfigData>(string name) where TConfigData : class, IConfigData
        {
            if (_configDataTable.ContainsKey(name))
            {
                return _configDataTable[name] as TConfigData;
            }
            else
            {
                // Load config data asset.
                string path = Path.Combine(DirectoryPath, name);
                var asset = Resources.Load<TextAsset>(path);

                Assert.IsNotNull(asset, $"[{nameof(ConfigManager)}] {path} 위치에 {name}.txt 에셋이 존재하지 않습니다.");

                // Convert asset text to config object.
                string jsonTxt = asset.text;
                TConfigData data = JsonUtility.FromJson<TConfigData>(jsonTxt);

                // Cache and return data.
                _configDataTable.Add(name, data);
                return data;
            }
        }
    }

    public interface IConfigData { }
}
