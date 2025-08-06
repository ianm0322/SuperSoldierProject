using System;
using UnityEngine;

namespace PrSuperSoldier
{
    public class PlayerLookController : MonoBehaviour
    {
        public Quaternion LookRotation { get; private set; }

        [HideInInspector]
        public Vector2 RotateInput;

        private Rigidbody _selfRigidbody;
        [SerializeField]
        private Transform _cameraTransform;

        [SerializeField]
        private float _sensitive = 1;
        [SerializeField]
        private float _pitchConstraintAngle = 89;

        private void Awake()
        {
            _selfRigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            LookRotation = _selfRigidbody.rotation;
        }

        public void LateUpdate()
        {
            float hRotating = _sensitive * RotateInput.x;
            float vRotating = _sensitive * -RotateInput.y;

            // control Yaw
            _selfRigidbody.rotation *= Quaternion.Euler(0, hRotating, 0);

            // control Pitch
            Vector3 currentCamAngles = _cameraTransform.localEulerAngles;
            float currentPitch = currentCamAngles.x;
            currentPitch += vRotating;
            currentPitch = Mathf.Clamp(currentPitch < 180 ? currentPitch : currentPitch - 360, -_pitchConstraintAngle, _pitchConstraintAngle);
            currentCamAngles.x = currentPitch;

            _cameraTransform.localEulerAngles = currentCamAngles;

            LookRotation = _cameraTransform.rotation;
        }
    }
}
