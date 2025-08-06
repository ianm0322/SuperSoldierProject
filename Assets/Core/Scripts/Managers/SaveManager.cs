using System.IO;
using UnityEngine;

namespace PrSuperSoldier
{
    public class SaveManager : Manager
    {
        private string _directoryPath;

        public override void Initialize()
        {
            var persistentPath = Application.persistentDataPath;
            var directoryName = GameManager.GetSubmanager<ConfigManager>().Get<ManagerConfigData>("manager-config").save_data_path;
            _directoryPath = Path.Combine(persistentPath, directoryName);
        }

        public void Save<T>(string name, T data) where T : class, IPlayerData
        {
            var saveFilePath = Path.Combine(_directoryPath, name);

            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }

            // Write save data.
            string content = JsonUtility.ToJson(data);

            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new FileStream(saveFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                sw = new StreamWriter(fs);

                sw.Write(content);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[{nameof(SaveManager)}] Save Failure.\n\nException Message: {e.Message}");
            }
            finally
            {
                sw?.Dispose();
                fs?.Dispose();
            }

            Debug.Log($"[{nameof(SaveManager)}] Save Succeeded!\n\nPath: {saveFilePath}");
        }

        public bool Load(string name)
        {
            return false;
        }

        public T Get<T>() where T : class, IPlayerData
        {
            return default(T);
        }
    }

    public interface IPlayerData { }
}
