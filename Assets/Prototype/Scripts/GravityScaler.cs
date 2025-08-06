using UnityEngine;

public class GravityScaler : MonoBehaviour
{
    public float GravityScale => _gravityScale;

    [SerializeField]
    private float _gravityScale;
    private Rigidbody _rigidbody;

    public void SetGravityScale(float scale)
    {
        _gravityScale = scale;
        if (Mathf.Approximately(scale, 1f))
        {
            _rigidbody.useGravity = true;
            this.enabled = false;
        }
        else
        {
            _rigidbody.useGravity = false;
            this.enabled = true;
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetGravityScale(_gravityScale);
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity += Physics.gravity * _gravityScale * Time.fixedDeltaTime;
    }
}
