using UnityEngine;
using UnityEngine.Events;

namespace PrSuperSoldier
{
    public class DestroyTrigger : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _onDestroyedEvent;

        private void OnDestroy()
        {
            _onDestroyedEvent?.Invoke();
        }
    }
}
