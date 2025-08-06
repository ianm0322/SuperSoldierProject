using UnityEngine;

public class Dummy : MonoBehaviour, IDamageable
{
    public void OnDamange()
    {
        Destroy(gameObject);
    }
}
