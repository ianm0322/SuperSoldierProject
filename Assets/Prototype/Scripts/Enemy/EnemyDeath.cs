using UnityEngine;

public class EnemyDeath : MonoBehaviour, IDamageable
{
    public void OnDamange()
    {
        Destroy(gameObject);
    }
}
