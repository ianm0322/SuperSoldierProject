using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace PrSuperSoldier
{
    public class PlayerHookController : MonoBehaviour
    {
        public bool IsHooked { get; private set; }

        public event Action OnHookingStart;
        public event Action OnHookingEnd;

        // Hook
        [SerializeField]
        private GameObject _hookPrefab;

        private CapsuleCollider _collider;
        private Rigidbody _rigidbody;
        private Player _player;
        private PlayerMovement _movement;
        private Transform _hookTransform;

        [Header("# Properties")]
        [field: SerializeField]
        public int MaxHookCount { get; set; } = 1;
        [field: SerializeField]
        public int CurrentHookCount { get; set; } = 0;

        [SerializeField]
        private float _distance;

        [SerializeField]
        private float _autoEndDistance = 2.5f;

        [Header("# Movement Settings")]
        [SerializeField]
        private float _speedOnWireAction;
        [SerializeField]
        private float _gravityScaleOnEndAction = 0.1f;
        [SerializeField]
        private float _gravityRevertSeconds = 0.5f;

        private bool _cancelGravityRevertTrigger;

        private RaycastHit _hookTestHitResult;
        private bool _canHookableCache;
        private int _hookableCheckFrame;

        public bool CanHookToTarget()
        {
            int currFrame = Time.frameCount;

            // Update canHookableCache
            if (currFrame != _hookableCheckFrame)
            {
                _hookableCheckFrame = currFrame;

                Ray ray = new Ray(_player.Camera.transform.position, _player.Camera.transform.forward);
                bool isHit = Physics.Raycast(ray, out _hookTestHitResult, _distance, LayerMask.GetMask(LayerDefines.Platform));

                _canHookableCache = isHit && _hookTestHitResult.collider.CompareTag("NotHookable") == false;
            }

            return _canHookableCache;
        }
        public bool IsLeftHookCount() => CurrentHookCount < MaxHookCount;

        public bool Hook()
        {
            if (IsHooked)
            {
                return false;
            }

            bool isHit = CanHookToTarget() && IsLeftHookCount();

            if (!isHit)
            {
                return false;
            }

            _hookTransform.transform.position = _hookTestHitResult.point;
            IsHooked = true;

            _movement.GravityScale = _gravityScaleOnEndAction;
            _cancelGravityRevertTrigger = true;

            ++CurrentHookCount;

            OnHookingStart?.Invoke();

            return true;
        }

        public void Cancel()
        {
            if (!IsHooked)
            {
                return;
            }

            IsHooked = false;
            _cancelGravityRevertTrigger = false;
            OnHookingEnd?.Invoke();
            ControlGravityAsync();
        }
        private async void ControlGravityAsync()
        {
            _movement.GravityScale = _gravityScaleOnEndAction;

            for (float timer = _gravityRevertSeconds; timer > 0; timer -= Time.deltaTime)
            {
                await UniTask.NextFrame();
                if (_cancelGravityRevertTrigger)
                {
                    break;
                }
            }

            _cancelGravityRevertTrigger = false;
            _movement.GravityScale = 1;
        }

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody>();
            _movement = GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            GameObject hookGO = Instantiate(_hookPrefab);
            _hookTransform = hookGO.transform;
        }

        private void OnEnable()
        {
            _movement.OnGrounded += FulfillHookAmmo;
        }
        private void FulfillHookAmmo()
        {
            CurrentHookCount = 0;
        }

        private void OnDestroy()
        {
            if (_hookTransform)
            {
                Destroy(_hookTransform.gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (IsHooked)
            {
                _movement.Mode = PlayerMovement.MovementMode.Fly;

                var closestPoint = _collider.ClosestPoint(_hookTransform.position);
                Vector3 delta = _hookTransform.position - closestPoint;
                float distance = delta.magnitude;
                if (distance < _autoEndDistance)
                {
                    Cancel();
                    return;
                }

                Vector3 moveForward = delta.normalized;
                Vector3 movement = moveForward;
                float moveDelta = _speedOnWireAction * Time.fixedDeltaTime;

                if (CastCollider(moveForward, 0.01f, out RaycastHit hit))
                {
                    movement = PhysicsMath.CalculateTangent(moveForward, hit.normal);
                }

                movement *= _speedOnWireAction;
                _rigidbody.linearVelocity = movement;
            }
            else
            {
                _movement.Mode = PlayerMovement.MovementMode.Walk;
            }
        }
        private bool CastCollider(Vector3 direction, float distance, out RaycastHit result)
        {
            Vector3 halfHeight = new Vector3(0, _collider.height * _collider.transform.lossyScale.y * 0.5f, 0);
            Vector3 p1 = _rigidbody.position + _collider.center + halfHeight;
            Vector3 p2 = _rigidbody.position + _collider.center - halfHeight;
            float radius = _collider.radius * _collider.transform.lossyScale.x;

            return Physics.CapsuleCast(p1, p2, radius, direction, out result, distance, LayerMask.GetMask(LayerDefines.Platform));
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _distance);

            if (IsHooked)
            {
                Gizmos.DrawWireSphere(_hookTransform.position, _autoEndDistance);
            }
        }
    }
}
