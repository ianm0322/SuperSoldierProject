using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace PrSuperSoldier
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private UIDocument _mainUIDocs;

        [SerializeField]
        private UIDocument _stageSelectUIDocs;

        [SerializeField]
        private MainMenuState _mainMenuStage;

        private UIDocument _currentUIDocs;

        private void Start()
        {
            GotoMainLobby();
            Debug.Log("Main Menu Initialized");
            Invoke(nameof(MoveScene), 3f);
        }
        private void MoveScene()
        {
            SceneManager.LoadScene("TestStage_001");
        }

        public void GotoStageSelect()
        {
            if (_currentUIDocs)
            {
                _currentUIDocs.gameObject.SetActive(false);
            }
            _stageSelectUIDocs.gameObject.SetActive(true);
            _currentUIDocs = _stageSelectUIDocs;
            _mainMenuStage = MainMenuState.StageSelect;

            Debug.Log($"Current Menu State: {_mainMenuStage}");
        }

        public void GotoMainLobby()
        {
            if (_currentUIDocs)
            {
                _currentUIDocs.gameObject.SetActive(false);
            }
            _mainUIDocs.gameObject.SetActive(true);
            _currentUIDocs = _mainUIDocs;
            _mainMenuStage = MainMenuState.MainMenu;

            Debug.Log($"Current Menu State: {_mainMenuStage}");
        }


        private enum MainMenuState
        {
            MainMenu,
            StageSelect
        }
    }
}
