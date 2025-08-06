using System;
using UnityEngine;

public class PlayerRotatingControl
{
    public Rigidbody SelfRigidbody;
    public Transform CameraTransform;

    public Vector2 RotateInput;
    private Settings _settings;

    public void OnUpdate()
    {
        float hRotating = _settings.Sensitive * RotateInput.x * Time.fixedDeltaTime;
        float vRotating = _settings.Sensitive * -RotateInput.y * Time.fixedDeltaTime;

        // control Yaw
        SelfRigidbody.rotation *= Quaternion.Euler(0, hRotating, 0);

        // control Pitch
        Vector3 currentCamAngles = CameraTransform.localEulerAngles;
        float currentPitch = currentCamAngles.x;
        currentPitch += vRotating;
        currentPitch = Mathf.Clamp(currentPitch < 180 ? currentPitch : currentPitch - 360, -_settings.PitchConstraintAngle, _settings.PitchConstraintAngle);
        currentCamAngles.x = currentPitch;

        CameraTransform.localEulerAngles = currentCamAngles;
    }

    public void SetSettings(Settings settings)
    {
        _settings = settings;
    }

    [Serializable]
    public struct Settings
    {
        public float Sensitive;
        public float PitchConstraintAngle;
    }
}