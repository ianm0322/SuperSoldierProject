using System.Threading;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gravityScaler = GetComponent<GravityScaler>();
    }

    private void Start()
    {
        if (TryGetComponent(out Player player))
        {
            _movement = player.Movement;
        }

        if (_hookObject == null)
        {
            GameObject go = new GameObject("Player Hooker");
            _hookObject = go.transform;
        }

        if (!_uiController.UIInstance.TryGetComponent(out _crosshairUI))
        {
            Debug.LogWarning("UIController에 할당된 UI가 crosshair가 아닙니다.");
        }
    }

    public void Hook()
    {
        _isHooking = false;
        bool isHit = Physics.Raycast(_fireTranform.position, _fireTranform.forward, out RaycastHit hit, _hookDistance, LayerMask.GetMask(LayerDefines.Platform));

        if (isHit)
        {
            // On Hooking Begin.
            _isHooking = true;
            _hookObject.position = hit.point;
            _gravityScaler.SetGravityScale(_gravityScaleOnHooking);
        }
    }

    public void Cut()
    {
        if (_isHooking)
        {
            _isHooking = false;
            RevertGravityAsync();
        }
    }

    private void Update()
    {
        if (_crosshairUI)
        {
            bool isHit = Physics.Raycast(_fireTranform.position, _fireTranform.forward, _hookDistance, LayerMask.GetMask(LayerDefines.Platform));
            _crosshairUI.IsHookable = isHit;
        }
    }

    private void FixedUpdate()
    {
        if (_isHooking)
        {
            Vector3 vector = _hookObject.position - this.transform.position;
            Vector3 direction = vector.normalized;
            _rigidbody.linearVelocity = direction * _hookActionSpeed;

            // On Hooking End
            if (vector.magnitude < _hookCutDistance)
            {
                _isHooking = false;
                RevertGravityAsync();
            }
        }
    }
    private async void RevertGravityAsync()
    {
        float timer = 0;
        while (timer < _gravityRevertTime)
        {
            if (_isHooking)
            {
                return;
            }
            else if (_movement.IsGrounded)
            {
                break;
            }

            await Awaitable.FixedUpdateAsync();
            timer += Time.fixedDeltaTime;
        }

        _gravityScaler.SetGravityScale(1f);
    }

    [SerializeField]
    private PlayerHUDController _uiController;
    private Crosshair _crosshairUI;

    [SerializeField]
    private Transform _fireTranform;
    [SerializeField]
    private Transform _hookObject;
    private Rigidbody _rigidbody;
    private GravityScaler _gravityScaler;
    private PlayerMovement _movement;

    [SerializeField]
    private float _gravityScaleOnHooking = 0.2f;

    [SerializeField]
    private float _gravityRevertTime = 0.5f;

    [SerializeField]
    private float _hookActionSpeed = 12f;

    [SerializeField]
    private float _hookCutDistance = 1.5f;

    [SerializeField]
    private float _hookDistance = 80;

    private bool _isHooking;
}
