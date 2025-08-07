using Cysharp.Threading.Tasks;
using PrSuperSoldier.Utilities;
using R3;
using R3.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace PrSuperSoldier
{
    public class PlayerMeleeController : MonoBehaviour 
    {
        public bool IsMeleeing => _isMeleeing;
        public float Duration => _meleeDuration;
        public float Delay => _meleeDelay;
        public float CooldownTime => Duration + Delay;

        public event Action OnMeleeExecute;


        [SerializeField]
        private Collider _meleeCollider;

        [SerializeField]
        private float _meleeDelay = 0.5f;

        [SerializeField]
        private float _meleeDuration = 1.5f;

        [SerializeField]
        private float _meleeDamage = 100f;

        [SerializeField]
        private float _meleeForce = 5f;

        [SerializeField]
        private float _moveSpeed = 10f;

        [SerializeField]
        private UnityEvent _onExecute;

        private bool _isMeleeing;
        private bool _canExecute = true;
        private float _bufferedSeconds;
        private static List<IDamageReceiveHandle> s_ReceiverHandleCache = new(8);

        private Rigidbody _rigidbody;
        private PlayerMovement _movement;
        private PlayerLookController _lookController;
        private CapsuleCollider _collider;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _movement = GetComponent<PlayerMovement>();
            _lookController = GetComponent<PlayerLookController>();
            _collider = GetComponent<CapsuleCollider>();
        }
        private void Start()
        {
            _meleeCollider.OnTriggerEnterAsObservable().Subscribe(OnHurtboxTriggered);

            _meleeCollider.enabled = false;
        }
        private void OnHurtboxTriggered(Collider result)
        {
            Debug.Log($"Melee attack triggered by: {result.name}");

            result.GetComponents(s_ReceiverHandleCache);
            if (s_ReceiverHandleCache.Count > 0)
            {
                Vector3 point = result.transform.position;
                Vector3 direction = transform.forward;
                foreach (var handle in s_ReceiverHandleCache)
                {
                    handle.OnDamageReceive(gameObject, result.gameObject, _meleeDamage, point, direction, _meleeForce);
                }
            }
        }

        public async UniTask Execute()
        {
            if (!_canExecute)
            {
                Debug.Log("Melee attack already in progress.");
                _bufferedSeconds = Time.time;
                return;
            }

            _meleeCollider.enabled = true;
            _isMeleeing = true;
            _canExecute = false;

            _onExecute?.Invoke();
            OnMeleeExecute?.Invoke();

            // 공격 중 움직임.
            Vector3 moveDir = _lookController.LookRotation * Vector3.forward;

            _movement.enabled = false;
            _movement.FlySpeed = _moveSpeed;
            _rigidbody.linearVelocity = Vector3.zero;
            var moveLoop = Observable.EveryUpdate(UnityFrameProvider.FixedUpdate).Subscribe((_) =>
            {
                _rigidbody.linearVelocity = moveDir * _moveSpeed;
            });

            await UniTask.WaitForSeconds(_meleeDuration);

            moveLoop.Dispose();

            _movement.enabled = true;
            _meleeCollider.enabled = false;
            _isMeleeing = false;

            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * 10;

            await UniTask.WaitForSeconds(_meleeDelay);

            _canExecute = true;

            if (Time.time - _bufferedSeconds < 0.1f)
            {
                Execute().Forget();
            }
        }
    }
}
