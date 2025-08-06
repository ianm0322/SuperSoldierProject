using PrSuperSoldier;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Quaternion LookRotation => _cameraTransform.rotation;
    public PlayerMovement Movement => _movement;

    [Header("# Input Actions")]
    [SerializeField]
    private InputActionReference _iaMove;
    [SerializeField]
    private InputActionReference _iaJump;
    [SerializeField]
    private InputActionReference _iaLook;
    [SerializeField]
    private InputActionReference _iaHook;
    [SerializeField]
    private InputActionReference _iaHookCut;
    [SerializeField]
    private InputActionReference _iaFire;


    private PlayerMovement _movement;
    private PlayerLookController _playerCamera;

    // components
    [Header("# Components")]
    private Rigidbody _rigidbody;
    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private PlayerHookController _hookController;
    [SerializeField]
    private PlayerShooter _shooter;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _movement = GetComponent<PlayerMovement>();
        _playerCamera = GetComponent<PlayerLookController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void OnEnable()
    {
        _iaMove.action.Enable();
        _iaJump.action.Enable();
        _iaLook.action.Enable();
        _iaHook.action.Enable();
        _iaHookCut.action.Enable();
        _iaFire.action.Enable();

        _iaHook.action.performed += HookInputPerformed;
        _iaHookCut.action.performed += HookCutInputPerformed;
        _iaFire.action.performed += FireInputPerformed;
    }

    private void FireInputPerformed(InputAction.CallbackContext obj)
    {
        _shooter.Shoot();
    }

    private void HookCutInputPerformed(InputAction.CallbackContext obj)
    {
        _hookController.Cut();
    }

    private void HookInputPerformed(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            _hookController.Hook();
        }
    }

    private void OnDisable()
    {
        _iaMove.action.Disable();
        _iaJump.action.Disable();
        _iaLook.action.Disable();
        _iaHook.action.Disable();
        _iaHookCut.action.Disable();
        _iaFire.action.Disable();

        _iaHook.action.performed -= HookInputPerformed;
        _iaHookCut.action.performed -= HookCutInputPerformed;
        _iaFire.action.performed -= FireInputPerformed;
    }

    private void Update()
    {
        _movement.MoveInput = _iaMove.action.ReadValue<Vector3>().normalized;
        _movement.IsJump = _iaJump.action.IsPressed();

        _playerCamera.RotateInput = _iaLook.action.ReadValue<Vector2>();
    }
}
