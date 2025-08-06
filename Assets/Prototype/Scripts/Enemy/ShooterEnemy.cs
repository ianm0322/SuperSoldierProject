using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    private GameObject _playerCharacter;

    [SerializeField]
    private float _effectiveDistance = 50;

    private ProjectileShooter _shooter;

    private void Start()
    {
        _playerCharacter = GameObject.FindWithTag("Player");
        _shooter = GetComponent<ProjectileShooter>();
    }

    private void Update()
    {
        Vector3 vector = _playerCharacter.transform.position - this.transform.position;
        float distance = vector.magnitude;

        if(distance > _effectiveDistance)
        {
            _shooter.enabled = false;
            return;
        }

        _shooter.enabled = true;

        Vector3 direction = vector.normalized;

        _shooter.FireTransform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _effectiveDistance);
    }
}
