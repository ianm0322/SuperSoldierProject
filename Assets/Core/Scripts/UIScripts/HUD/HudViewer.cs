using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PrSuperSoldier.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class HudViewer : MonoBehaviour
    {
        public VisualElement Root => _root;
        public VisualElement Crosshair => _crosshair;
        public VisualElement AmmoGroup => _ammoGroup;
        public IReadOnlyList<VisualElement> AmmoDisplays => _ammoDisplays;
        public Label TimerText => _timerText;
        public VisualElement MeleeCooldownBar => _meleeCooldownBar;

        private UIDocument _uiDocs;
        private VisualElement _root;
        private VisualElement _crosshair;
        private VisualElement _ammoGroup;
        private List<VisualElement> _ammoDisplays = new List<VisualElement>();
        private Label _timerText;
        private VisualElement _meleeCooldownBar;

        private HudPresenter _presenter;

        private void Start()
        {
            _uiDocs = GetComponent<UIDocument>();

            // UI 요소 초기화
            _root = _uiDocs.rootVisualElement;
            _crosshair = _root.Q<VisualElement>("crosshair");
            _ammoGroup = _root.Q<VisualElement>("ammo-group");
            for (int i = 0; i < _ammoGroup.childCount; ++i)
            {
                _ammoDisplays.Add(_ammoGroup[i]);
            }
            _timerText = _root.Q<Label>("timer-text");
            _meleeCooldownBar = _root.Q<VisualElement>("melee-progress-bar");

            SetupPresenter();
        }
        private async void SetupPresenter()
        {
            var levelManager = await GameManager.GetSubmanagerAsync<LevelManager>();

            _presenter = new HudPresenter(this, levelManager.Player);
            _presenter.OnEnabled();
        }

        private void OnEnable()
        {
            if (this.didStart)
            {
                _presenter.OnEnabled();
            }
        }
        private void OnDisable()
        {
            _presenter.OnDisabled();
        }
    }
}