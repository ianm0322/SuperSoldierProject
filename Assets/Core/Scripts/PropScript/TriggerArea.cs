using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PrSuperSoldier
{
    [RequireComponent(typeof(Collider))]
    public class TriggerArea : MonoBehaviour
    {
        private Collider _collider;

        [SerializeField]
        private UnityEvent<Collider> _onEnter;
        [SerializeField]
        private UnityEvent<Collider> _onExit;

        private void Awake()
        {
            _collider = GetComponent<Collider>();

            var enterListeners = GetComponents<ITriggerEnterEventHandler>();
            foreach (var item in enterListeners)
            {
                _onEnter.AddListener(item.OnCollisionEnter);
            }

            var exitListeners = GetComponents<ITriggerExitEventHandler>();
            foreach(var item in exitListeners)
            {
                _onExit.AddListener(item.OnCollisionExit);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _onEnter.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _onExit.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {

        }
        private void OnValidate()
        {
            var collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }
    }

    public interface ITriggerEnterEventHandler
    {
        public void OnCollisionEnter(Collider other);
    }
    public interface ITriggerExitEventHandler
    {
        public void OnCollisionExit(Collider other);
    }
}
