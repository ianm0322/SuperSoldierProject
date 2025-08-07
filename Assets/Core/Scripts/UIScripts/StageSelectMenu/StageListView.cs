using UnityEngine;
using UnityEngine.UI;

namespace PrSuperSoldier.UI
{
    public class StageListView : MonoBehaviour
    {
        public ScrollRect Scroll => _scroll;

        [SerializeField, HideInInspector]
        private ScrollRect _scroll;

        private void OnValidate()
        {
            if (_scroll == null)
            {
                _scroll = GetComponent<ScrollRect>();
            }
        }
    }
}
