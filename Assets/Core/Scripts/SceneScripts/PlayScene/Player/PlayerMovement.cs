using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem.iOS;

namespace PrSuperSoldier
{
    public class PlayerMovement : MonoBehaviour
    {
        public event Action OnGrounded;
        public event Action OnFallBegin;

        public bool IsGrounded { get; private set; }
        public bool IsWallHit => _isWallHit;
        public MovementMode Mode { get; set; }
        public float GravityScale
        {
            get => _gravityScale;
            set => _gravityScale = value;
        }

        [HideInInspector]
        public Vector3 MoveInput;
        [HideInInspector]
        public bool IsJump;



        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;

        [Header("# Settings")]
        [SerializeField]
        private Settings _settings;

        [field: SerializeField]
        public float FlySpeed { get; set; } = 10f;

        [Header("# States")]
        [field: SerializeField]
        public bool AlignToRotation { get; set; } = true;

        private RaycastHit _groundHitResult;
        private RaycastHit _wallHitResult;

        private bool _isWallHit;

        [SerializeField]
        private float _gravityScale = 1f;

        private float _bodyRadius;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        }

        private void OnEnable()
        {
            _bodyRadius = _collider.radius * transform.localScale.x;
        }

        private void OnValidate()
        {
            if (TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.useGravity = false;
            }
        }

        private void FixedUpdate()
        {
            UpdatePhysicsMaterials();
            CheckGroundCollision();
            CheckWallCollision();
            UpdateGravity();
            UpdateMovement();
        }

        [MethodImpl]
        private void UpdatePhysicsMaterials()
        {
            if (Mode == MovementMode.Fly)
            {
                _collider.material = _settings.PMOnAir;
                return;
            }
            else
            {
                _collider.material = IsGrounded ? _settings.PMOnGround : _settings.PMOnAir;
            }
        }
        [MethodImpl]
        private void CheckGroundCollision()
        {
            // 상태 체크.
            if (Mode == MovementMode.Walk)
            {
                bool grounded = Physics.SphereCast(_settings.BottomTransform.position,
                                            _bodyRadius,
                                            Vector3.down,
                                            out _groundHitResult,
                                            _settings.GroundingDistance,
                                            LayerMask.GetMask(LayerDefines.Platform, "Default"));

                if (!IsGrounded && grounded)
                {
                    OnGrounded?.Invoke();
                }
                else if (IsGrounded && !grounded)
                {
                    OnFallBegin?.Invoke();
                }

                IsGrounded = grounded;
            }
            else
            {
                IsGrounded = false;
            }
        }
        [MethodImpl]
        private void CheckWallCollision()
        {
            if (MoveInput.sqrMagnitude < 0.001f)
            {
                _isWallHit = false;
                return;
            }

            Vector3 p1 = transform.TransformPoint(_collider.bounds.center) + Vector3.up * _collider.height * transform.lossyScale.y * 0.5f;
            Vector3 p2 = transform.TransformPoint(_collider.bounds.center) - Vector3.up * _collider.height * transform.lossyScale.y * 0.5f;
            _isWallHit = Physics.CapsuleCast(p1,
                                            p2,
                                            _bodyRadius,
                                            MoveInput.normalized,
                                            out _wallHitResult,
                                            Physics.defaultContactOffset,
                                            LayerMask.GetMask(LayerDefines.Platform, "Default"));
        }
        [MethodImpl]
        private void UpdateGravity()
        {
            if (Mode != MovementMode.Fly)
            {
                Vector3 appliedGravity = Physics.gravity * _gravityScale;
                _rigidbody.AddForce(appliedGravity, ForceMode.Acceleration);
            }
        }
        [MethodImpl]
        private void UpdateMovement()
        {
            MoveInput.Normalize();
            Vector3 direction = AlignToRotation ? _rigidbody.rotation * MoveInput : MoveInput;
            if (Mode == MovementMode.Walk)
            {
                // 수직방향 무시.
                direction.y = 0;
                direction.Normalize();

                // 이동
                if (IsGrounded)
                {
                    Vector3 velocity = _rigidbody.linearVelocity;
                    float yVelocity = velocity.y;

                    velocity = direction * _settings.SpeedOnGround;
                    velocity.y = yVelocity;

                    _rigidbody.linearVelocity = velocity;

                    //// 마찰력 조절
                    //_collider.material = _settings.PMOnGround;
                }
                else
                {
                    _rigidbody.AddForce(direction * _settings.AccelOnAir, ForceMode.Acceleration);

                    //// 마찰력 조절
                    //_collider.material = _settings.PMOnAir;
                }

                // 점프
                if (IsJump && IsGrounded)
                {
                    _rigidbody.AddForce(Vector3.up * _settings.JumpPower, ForceMode.VelocityChange);
                }
            }
            else if (Mode == MovementMode.Fly)
            {
                //if (_isWallHit)
                //{
                //    direction = PhysicsMath.CalculateTangent(direction, _wallHitResult.normal);
                //}

                _rigidbody.AddForce(direction * FlySpeed, ForceMode.Acceleration);
            }
        }

        [Serializable]
        public struct Settings
        {
            public Transform BottomTransform;
            public float GroundingDistance;

            [Header("# On Ground")]
            public float SpeedOnGround;

            [Header("# On Air")]
            public float AccelOnAir;

            [Header("# Jump")]
            public float JumpPower;

            [Header("# Physics Materials")]
            public PhysicsMaterial PMOnGround;
            public PhysicsMaterial PMOnAir;
        }

        public enum MovementMode
        {
            Walk,
            Fly
        }
    }
}
