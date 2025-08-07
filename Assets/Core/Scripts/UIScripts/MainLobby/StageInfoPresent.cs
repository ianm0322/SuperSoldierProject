using UnityEngine;
using UnityEngine.SceneManagement;

namespace PrSuperSoldier.UI
{
    public class StageInfoPresent : MonoBehaviour
    {
        public StageInfoSO stageInfo;
        private StageInfoView _view;

        private void Awake()
        {
            _view = GetComponent<StageInfoView>();

            _view.Button.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(stageInfo.sceneName);
            });
        }

        private void OnEnable()
        {
            if (this.didStart)
            {
                Apply();
            }
        }

        private void Start()
        {
            Apply();
        }

        public void Apply()
        {
            if (stageInfo == null)
            {
                Debug.LogError("StageInfoSO is not assigned in StageInfoPresent.");
                _view.StageNameText.text = "No named";
                _view.StageRecordText.text = @"-:-.-";
                _view.StageThumbnailImage.sprite = null;
                _view.Button.interactable = true;
                return;
            }
            _view.StageNameText.text = stageInfo.title;
            _view.StageRecordText.text = stageInfo.bestRecord.ToString(@"mm:ss.ff");
            _view.StageThumbnailImage.sprite = stageInfo.thumbnail;
            _view.Button.interactable = !stageInfo.isLocked;
        }
    }
}
