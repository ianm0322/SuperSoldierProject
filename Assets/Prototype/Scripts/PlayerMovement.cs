using System;
using UnityEngine;

public class PlayerMovement
{
    public bool IsGrounded { get; private set; }

    public PlayerMovement(Rigidbody rigidbody, Settings settings)
    {
        _rigidbody = rigidbody;
        _collider = rigidbody.GetComponent<Collider>();
        _settings = settings;
    }

    public void SetSettings(in Settings settings)
    {
        _settings = settings;
    }

    public void OnUpdate()
    {
        // 상태 체크.
        bool isGround = Physics.SphereCast(_settings.BottomTransform.position,
                                        _settings.BodyRadius,
                                        Vector3.down,
                                        out RaycastHit result,
                                        _settings.GroundingDistance,
                                        LayerMask.GetMask(LayerDefines.Platform));

        IsGrounded = isGround;

        // 이동
        if (isGround)
        {
            Vector3 velocity = _rigidbody.linearVelocity;
            float gravity = velocity.y;

            Vector3 forward = _rigidbody.rotation * Vector3.forward;
            Vector3 right = _rigidbody.rotation * Vector3.right;
            Vector3 direction = forward * MoveInput.z + right * MoveInput.x;

            velocity = direction * _settings.SpeedOnGround;
            velocity.y = gravity;

            _rigidbody.linearVelocity = velocity;

            // 마찰력 조절
            _collider.material = _settings.PMOnGround;
        }
        else
        {
            Vector3 forward = _rigidbody.rotation * Vector3.forward;
            Vector3 right = _rigidbody.rotation * Vector3.right;
            Vector3 direction = forward * MoveInput.z + right * MoveInput.x;

            _rigidbody.AddForce(direction * _settings.AccelOnAir, ForceMode.Acceleration);

            // 마찰력 조절
            _collider.material = _settings.PMOnAir;
        }

        // 점프
        if (IsJump && isGround)
        {
            _rigidbody.AddForce(Vector3.up * _settings.JumpPower, ForceMode.VelocityChange);
        }
    }

    private Rigidbody _rigidbody;
    private Collider _collider;
    private Settings _settings;

    public Vector3 MoveInput;
    public bool IsJump;

    [Serializable]
    public struct Settings
    {
        public Transform BottomTransform;
        public float BodyHeight;
        public float BodyRadius;
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
}
