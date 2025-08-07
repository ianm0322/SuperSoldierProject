using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Assertions;

namespace PrSuperSoldier.Props
{
    public class Door : MonoBehaviour
    {
        // ���� ���� �����ִ��� ����.
        public bool IsOpen => _isOpen;
        // ���� �Լ��� ���� ������� ����.
        public bool Locked
        {
            get => _locked;
            set => _locked = value;
        }
        /// <summary>
        /// ���� Progress ��.<br/>
        /// T ���� 0�̸� ����, 1�̸� ����.
        /// </summary>
        public float T
        {
            get => _t;
            set
            {
                Assert.IsFalse(AnimationRunning, "�� �ִϸ��̼��� ���� ���� �� ���� ������ �� �����ϴ�!");

                _t = value;
                _isOpen = _t == 0f ? false : _t == 1f ? true : _isOpen;
                ApplyDoorPosition();
            }
        }
        /// <summary>
        /// ���� �ִϸ��̼� �ӵ�.
        /// </summary>
        public float Rate
        {
            get => _rate;
            set => _rate = value;
        }

        // �� �ִϸ��̼��� ���� ������ ����.
        private bool AnimationRunning => _animationTask.Status == UniTaskStatus.Pending;


        [Header("# Properties")]
        [SerializeField]
        private bool _openOnAwake;

        // T ���� ���� ���� ����.
        [SerializeField]
        private bool _locked = false;

        [SerializeField]
        private float _rate = 3;
        [SerializeField]
        private float _t;

        [Header("# References")]
        [SerializeField]
        private GameObject _doorObject;
        [SerializeField, HideInInspector]
        private Transform _doorTransform;
        [SerializeField, HideInInspector]
        private Collider _doorCollider;
        [Header("# Offset")]
        [SerializeField]
        private Vector3 _openOffset = new Vector3(1, 0, 0);
        [SerializeField]
        private Vector3 _closeOffset = Vector3.zero;


        private bool _isOpen;
        private UniTask _animationTask;


        private void OnValidate()
        {
            if (_openOnAwake)
            {
                _t = 1f; // �ʱ�ȭ �� ���� �����
            }
            else
            {
                _t = 0f; // �ʱ�ȭ �� ���� �ݾƵ�
            }

            _doorTransform = _doorObject.transform;
            _doorCollider = _doorObject.GetComponent<Collider>();

            ApplyDoorPosition();
        }

        private void Start()
        {
            if (_openOnAwake)
            {
                OpenImmediately();
            }

            ApplyDoorPosition();
        }

        private void Update()
        {
            ApplyDoorPosition();
        }

        [ContextMenu("# Run Open Anim")]
        public void Open()
        {
            if (!_locked && !_isOpen)
            {
                _isOpen = true;
                _doorCollider.enabled = false;
                _animationTask = OpenAnimationAsync();
            }
        }
        public void OpenImmediately()
        {
            _isOpen = true;
            _doorCollider.enabled = false;
            _t = 1f;
            // ApplyDoorPosition();
        }
        private async UniTask OpenAnimationAsync()
        {
            while (_t < 1f && _isOpen)
            {
                await UniTask.NextFrame();

                _t = Mathf.MoveTowards(_t, 1f, Time.deltaTime * _rate);
                // ApplyDoorPosition();
            }
        }

        [ContextMenu("# Run Close Anim")]
        public void Close()
        {
            if (!_locked && _isOpen)
            {
                _isOpen = false;
                _doorCollider.enabled = true;
                _animationTask = CloseAnimationAsync();
            }
        }
        public void CloseImmediately()
        {
            _isOpen = false;
            _doorCollider.enabled = true;
            _t = 0;
            // ApplyDoorPosition();
        }
        private async UniTask CloseAnimationAsync()
        {
            while (_t > 0f && !_isOpen)
            {
                await UniTask.NextFrame();

                _t = Mathf.MoveTowards(_t, 0f, Time.deltaTime * _rate);
                // ApplyDoorPosition();
            }
        }
        private void ApplyDoorPosition()
        {
            Assert.IsNotNull(_doorTransform, "_doorTransform �Ҵ���� ����!");   // Assertion: _doorTransform �Ҵ���� ����!
            _doorTransform.localPosition = Vector3.LerpUnclamped(_closeOffset, _openOffset, _t);
        }

        private void OnDrawGizmos()
        {
            if (!enabled)
                return;

            if (_doorObject && _doorObject.TryGetComponent(out MeshFilter meshFilter))
            {
                Vector3 closedPosition = transform.position + _closeOffset;
                Vector3 openPosition = transform.position + _openOffset;

                Gizmos.color = Color.red;
                Gizmos.DrawWireMesh(meshFilter.sharedMesh, closedPosition, _doorTransform.rotation, _doorTransform.lossyScale);

                Gizmos.color = Color.green;
                Gizmos.DrawWireMesh(meshFilter.sharedMesh, openPosition, _doorTransform.rotation, _doorTransform.lossyScale);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(closedPosition, openPosition);
            }
            else
            {
                Vector3 closedPosition = transform.position + _closeOffset;
                Vector3 openPosition = transform.position + _openOffset;

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(closedPosition, 0.5f);

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(openPosition, 0.5f);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(closedPosition, openPosition);
            }
        }

        private enum State
        {
            None,
            Closed,
            Open,
            CloseToOpen,
            OpenToClose,
        }
    }
}
