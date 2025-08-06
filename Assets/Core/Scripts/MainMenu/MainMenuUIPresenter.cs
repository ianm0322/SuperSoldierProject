using UnityEngine;
using UnityEngine.UIElements;

namespace PrSuperSoldier
{
    public class MainMenuUIPresenter : MonoBehaviour
    {
        [SerializeField]
        private MainMenuManager _manager;

        UIDocument _view;

        VisualElement _root;
        Button _continueButton;
        Button _selectStageButton;
        Button _chellangeModeButton;
        Button _optionsButton;
        Button _quitButton;

        private void Awake()
        {
            _view = GetComponent<UIDocument>();
        }

        private void Start()
        {
            _root = _view.rootVisualElement;

            _continueButton = _root.Q<Button>("continue-button");
            _selectStageButton = _root.Q<Button>("select-stage-button");
            _chellangeModeButton = _root.Q<Button>("chellange-mode-button");
            _optionsButton = _root.Q<Button>("option-button");
            _quitButton = _root.Q<Button>("quit-button");

            _continueButton.clicked += () =>
            {
                Debug.Log("Continue button clicked");
            };
            _selectStageButton.clicked += () =>
            {
                Debug.Log("Select Stage button clicked");
                _manager.GotoStageSelect();
            };
            _chellangeModeButton.clicked += () =>
            {
                Debug.Log("Challenge Mode button clicked");
            };
            _optionsButton.clicked += () =>
            {
                Debug.Log("Options button clicked");
            };
            _quitButton.clicked += () =>
            {
                Debug.Log("Quit button clicked");
                Application.Quit();
            };
        }
    }
}
