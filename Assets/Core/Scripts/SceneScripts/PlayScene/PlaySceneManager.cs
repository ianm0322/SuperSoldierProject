using R3;
using System;
using UnityEngine;

namespace PrSuperSoldier
{
    public class PlaySceneManager : MonoSingleton<PlaySceneManager>
    {
        public float ElapsedTime => _elapsedTime;
        public bool IsPaused { get; set; }
        public bool IsPlaying { get; private set; }

        public GameObject Player => _player;

        [SerializeField]
        private GameObject _player;

        private float _elapsedTime;

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            _elapsedTime = 0f;
            IsPlaying = true;
        }

        public void EndGame()
        {
            IsPlaying = false;
        }

        private void Update()
        {
            UpdateTime();
        }
        private void UpdateTime()
        {
            if (!IsPlaying || IsPaused)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;
        }
    }
}
