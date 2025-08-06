using UnityEngine;
using UnityEngine.SceneManagement;

namespace PrSuperSoldier
{
    public class LevelLoadManager : MonoBehaviour
    {
        private static LevelLoadManager s_Instance;
        public static LevelLoadManager Instance => s_Instance;

        private Scene _loadedLevel;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public async void LoadLevelAsync(string sceneName)
        {
            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            //op.
            //SceneManager.scene
        }
    }
}
