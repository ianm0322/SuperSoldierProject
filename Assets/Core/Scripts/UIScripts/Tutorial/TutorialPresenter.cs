using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace PrSuperSoldier
{
    public class TutorialPresenter : MonoBehaviour
    {
        private const string DisabledAnimationClass = "disabled";

        private UIDocument _uiDocument;

        private VisualElement _root;
        private Label _textLabel;

        [SerializeField]
        private string _text;
        [SerializeField]
        private bool _once;
        [SerializeField]
        private float _duration = -1f;

        private bool _active = false;
        private bool _wasShown = false;

        private void Awake()
        {
            InitializeUIElements();
        }
        private void InitializeUIElements()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            _textLabel = _root.Q<Label>("tutorial-text");

            _textLabel.RegisterCallbackOnce<TransitionEndEvent>(DisableDocument);
        }

        public void Show()
        {
            if(_once && _wasShown)
            {
                return;
            }

            _uiDocument.enabled = true;
            _textLabel.text = _text;
            this.enabled = true;
            _wasShown = true;

            _textLabel.RemoveFromClassList(DisabledAnimationClass);

            if(_duration > 0f)
            {
                AutoDisableAsync();
            }
        }
        private async void AutoDisableAsync()
        {
            await UniTask.WaitForSeconds(_duration);
            Hide();
        }
      
        public void Hide()
        {
            this.enabled = false;
            _textLabel.AddToClassList(DisabledAnimationClass);
        }

        private void DisableDocument(TransitionEndEvent evt)
        {
            // 사라지는 애니메이션 트랜지션 종료가 아니라면 함수 종료.
            if (!_textLabel.ClassListContains(DisabledAnimationClass))
            {
                return;
            }

            _uiDocument.enabled = false;
            if (_wasShown && _once)
            {
                Destroy(gameObject);
            }
        }
    }
}
