using Unity.Cinemachine;
using UnityEngine;

namespace PrSuperSoldier
{
    public class Player : MonoBehaviour
    {
        public CinemachineCamera Camera => _playerCamera;

        [SerializeField]
        private CinemachineCamera _playerCamera;
    }
}
