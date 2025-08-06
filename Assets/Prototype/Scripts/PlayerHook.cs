using System;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public event Action OnHooked;

    public void Shoot(Vector3 position, Quaternion rotation)
    {
        _rigidbody.position = position;
        _rigidbody.rotation = rotation;

        enabled = true;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = _rigidbody.rotation * Vector3.forward;
        _rigidbody.MovePosition(_rigidbody.position + direction * _moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        enabled = false;
    }

    private Rigidbody _rigidbody;
    [SerializeField]
    private float _moveSpeed = 80;
}
