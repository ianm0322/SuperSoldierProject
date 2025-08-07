using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PrSuperSoldier.UI
{
    public class StageInfoView : MonoBehaviour
    {
        [Header("# Inputs")]
        [SerializeField]
        private Button _button;

        [Header("# Outputs")]
        [SerializeField]
        private TMP_Text _stageNameText;
        [SerializeField]
        private TMP_Text _stageRecordText;
        [SerializeField]
        private Image _stageThumbnailImage;


        // 프로퍼티를 통해 외부에서 접근할 수 있도록 함
        public TMP_Text StageNameText => _stageNameText;
        public TMP_Text StageRecordText => _stageRecordText;
        public Image StageThumbnailImage => _stageThumbnailImage;
        public Button Button => _button;
    }
}
