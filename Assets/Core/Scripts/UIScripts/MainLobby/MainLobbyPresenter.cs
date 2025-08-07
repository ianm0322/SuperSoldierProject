using UnityEngine;

namespace PrSuperSoldier.UI
{
    public class MainLobbyPresenter : MonoBehaviour
    {
        private MainLobbyView _view;

        private MainMenuManager _mainMenuManager;

        private void Awake()
        {
            _view = GetComponent<MainLobbyView>();

            _view.StoryModeButton.onClick.AddListener(() => { _mainMenuManager.GotoStageSelect(); });
        }
    }
}
