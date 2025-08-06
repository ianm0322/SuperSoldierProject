using UnityEngine;
using UnityEngine.SceneManagement;

namespace PrSuperSoldier
{
    public class TestSceneLoader : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene("TestStage_001");
        }
    }
}
