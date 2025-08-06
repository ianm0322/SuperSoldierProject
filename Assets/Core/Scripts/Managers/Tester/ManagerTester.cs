using System;
using UnityEngine;

namespace PrSuperSoldier
{
    public class ManagerTester : MonoBehaviour
    {
        private void Update()
        {
            if (GameManager.Initialized)
            {
                enabled = false;

                // Test 0. Json
                Debug.Log(JsonUtility.ToJson(new ManagerConfigData() { save_data_path = "Test" }, true));

                // Test 1. config manager.
                var configManager = GameManager.GetSubmanager<ConfigManager>();
                var result = configManager.Get<ManagerConfigData>("manager-config").save_data_path;
                Debug.Log($"Test 1: manager-config.save_data_path = {result}");

                // Test 2. save manager.
                var saveManager = GameManager.GetSubmanager<SaveManager>();
                saveManager.Save("player-save-data.txt", new TestSaveData() { name = "Yeonggoen", age = 25 });
            }
        }
    }

    [Serializable]
    public class TestSaveData : IPlayerData
    {
        public string name;
        public int age;
    }
}
