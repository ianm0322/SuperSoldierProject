using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rigidbody.linearVelocity = _rigidbody.rotation * Vector3.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is IDamageable damageable)
        {
            damageable.OnDamange();
        }
        Destroy(gameObject);
    }

    [SerializeField]
    private float _speed;

    private Rigidbody _rigidbody;
}
