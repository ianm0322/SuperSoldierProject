using UnityEngine;
using UnityEngine.SceneManagement;

namespace PrSuperSoldier
{
    public class PlayStageLoader : MonoBehaviour
    {
        [SerializeField]
        private string _levelPrefix = "SsLevel_";

        public void LoadLevel(int stateID)
        {
            string levelName = _levelPrefix + stateID.ToString();
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }
    }
}
