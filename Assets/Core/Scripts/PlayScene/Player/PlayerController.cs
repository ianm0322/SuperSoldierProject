using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace PrSuperSoldier
{
    public class PlayerController : MonoBehaviour
    {
        public bool UseMovementControl { get; set; } = true;
        public CinemachineCamera PlayerCamera => _playerCamera;

        [Header("# Input")]
        [SerializeField]
        private InputActionAsset _inputSchema;

        private InputAction _iaMove;
        private InputAction _iaJump;
        private InputAction _iaLook;
        private InputAction _iaHook;
        private InputAction _iaHookCut;
        private InputAction _iaFire;
        private InputAction _iaMelee;

        private PlayerMovement _movement;
        private PlayerLookController _lookController;
        private PlayerHookController _hookController;
        private PlayerMeleeController _meleeController;

        [Header("# References")]
        [SerializeField]
        private CinemachineCamera _playerCamera;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _lookController = GetComponent<PlayerLookController>();
            _hookController = GetComponent<PlayerHookController>();
            _meleeController = GetComponent<PlayerMeleeController>();

            SetupInputAction();
        }

        private void SetupInputAction()
        {
            Assert.IsNotNull(_inputSchema);

            _iaMove = _inputSchema.FindAction("Move");
            _iaJump = _inputSchema.FindAction("Jump");
            _iaLook = _inputSchema.FindAction("Look");
            _iaHook = _inputSchema.FindAction("Hook");
            _iaHookCut = _inputSchema.FindAction("Cancel Hook");
            _iaFire = _inputSchema.FindAction("Attack");
            _iaMelee = _inputSchema.FindAction("Melee");

            _iaHook.performed += HookInputPerformed;
            _iaHookCut.performed += HookCutInputPerformed;
            _iaFire.performed += FireInputPerformed;
            _iaMelee.performed += MeleeInputPerformed;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            _inputSchema.Enable();

            _hookController.OnHookingEnd += () =>
            {
                UseMovementControl = true;
            };
        }

        private void OnEnable()
        {
            _inputSchema.Enable();

            Debug.Log("PlayerController enabled");
        }

        private void OnDisable()
        {
            //_inputSchema.Disable();

            Debug.Log("PlayerController disabled");
        }

        private void MeleeInputPerformed(InputAction.CallbackContext context)
        {
            _meleeController.Execute().Forget();
        }

        private void FireInputPerformed(InputAction.CallbackContext obj)
        {
        }

        private void HookCutInputPerformed(InputAction.CallbackContext obj)
        {
            _hookController.Cancel();
        }

        private void HookInputPerformed(InputAction.CallbackContext obj)
        {
            bool isHooking = _hookController.Hook();
            if (isHooking)
            {
                UseMovementControl = false;
            }
        }

        private void Update()
        {
            if (UseMovementControl)
            {
                _movement.MoveInput = _iaMove.ReadValue<Vector3>().normalized;
                _movement.IsJump = _iaJump.IsPressed();
            }
            else
            {
                _movement.MoveInput = Vector3.zero;
                _movement.IsJump = false;
            }

            _lookController.RotateInput = _iaLook.ReadValue<Vector2>();
        }
    }
}
