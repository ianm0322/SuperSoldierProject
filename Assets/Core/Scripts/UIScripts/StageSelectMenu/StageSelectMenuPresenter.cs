using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace PrSuperSoldier
{
    public class StageSelectMenuPresenter : MonoBehaviour
    {
        private UIDocument _uiDocument;

        private VisualElement _root;
        private ScrollView _stageList;

        public StageTableSO stage;
        public VisualTreeAsset elemTemplate;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            _root = _uiDocument.rootVisualElement;
            _stageList = _root.Q<ScrollView>();

        }
        private void OnEnable()
        {
            ListSetup();
        }
        private void ListSetup()
        {
            // 이전 리스트 삭제.
            _stageList.contentContainer.Clear();

            foreach(var stage in stage.values)
            {
                var elem = InstantiateListElement(stage);
                _stageList.contentContainer.Add(elem);
            }
        }
        private VisualElement InstantiateListElement(StageInfoSO stageInfo)
        {
            var listElement = elemTemplate.Instantiate();
            listElement.dataSource = stageInfo;

            var button = listElement.Q<Button>();
            button.clicked += () =>
            {
                Debug.Log($"Loading scene: {stageInfo.sceneName}");
                LoadScene(stageInfo.sceneName);
            };

            return listElement;
        }
        private async void LoadScene(string sceneName)
        {
            var op = SceneManager.LoadSceneAsync(sceneName);
            await op.ToUniTask();
        }
    }
}
