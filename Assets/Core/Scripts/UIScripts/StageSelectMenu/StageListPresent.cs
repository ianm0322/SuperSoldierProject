using UnityEngine;

namespace PrSuperSoldier.UI
{
    public class StageListPresent : MonoBehaviour
    {
        private StageListView _view;

        private void Awake()
        {
            _view = GetComponent<StageListView>();
        }

        private void OnEnable()
        {
            _view.Scroll.horizontalNormalizedPosition = 0f;
        }
    }
}
