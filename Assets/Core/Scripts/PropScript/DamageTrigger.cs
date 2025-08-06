using UnityEngine;
using UnityEngine.Events;

namespace PrSuperSoldier
{
    public class DamageTrigger : MonoBehaviour, IDamageReceiveHandle
    {
        [SerializeField]
        private UnityEvent<float, Vector3, Vector3, float> onDamageReceive;

        public void OnDamageReceive(GameObject attacker, GameObject receiver, float damage, Vector3 hitPoint, Vector3 hitDirection, float hitForce)
        {
            onDamageReceive?.Invoke(damage, hitPoint, hitDirection, hitForce);
        }
    }
}
