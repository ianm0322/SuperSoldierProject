using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileShooter : MonoBehaviour
{
    public Transform FireTransform => _fireTransform;


    public GameObject ProjectilePrefab;
    public float Interval = 1f;
    public float Rate = 1f;

    [SerializeField]
    private Transform _fireTransform;
    [SerializeField]
    private bool _resetOnEnabled = true;

    private float _fireTimer;

    private void OnEnable()
    {
        if (_resetOnEnabled)
        {
            _fireTimer = 0;
        }
    }

    private void Update()
    {
        Assert.IsTrue(Interval > 0);

        _fireTimer += Time.deltaTime * Rate;

        if(_fireTimer > Interval)
        {
            _fireTimer -= Interval;
            Fire();
        }
    }
    private void Fire()
    {
        GameObject inst = Instantiate(ProjectilePrefab);

        inst.transform.position = _fireTransform.position;
        inst.transform.rotation = _fireTransform.rotation;
    }
}
