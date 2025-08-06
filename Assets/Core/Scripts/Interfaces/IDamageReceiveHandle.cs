using UnityEngine;

namespace PrSuperSoldier
{
    public interface IDamageReceiveHandle
    {
        public void OnDamageReceive(
            GameObject attacker,
            GameObject receiver,
            float damage,
            Vector3 hitPoint,
            Vector3 hitDirection,
            float hitForce);
    }
}
