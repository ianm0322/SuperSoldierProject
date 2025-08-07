using UnityEngine;
using UnityEngine.UI;

namespace PrSuperSoldier.UI
{
    public class MainLobbyView : MonoBehaviour
    {
        // 버튼 필드에 대한 프로퍼티
        public Button ContinueButton => _continueButton;
        public Button StoryModeButton => _storyModeButton;
        public Button ChallengeModeButton => _chellangeModeButton;
        public Button SettingsButton => _settingsButton;
        public Button ExitButton => _exitButton;

        [SerializeField]
        private Button _continueButton;
        [SerializeField]
        private Button _storyModeButton;
        [SerializeField]
        private Button _chellangeModeButton;
        [SerializeField]
        private Button _settingsButton;
        [SerializeField]
        private Button _exitButton;
    }
}
