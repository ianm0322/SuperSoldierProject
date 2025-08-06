using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrSuperSoldier
{
    public class GameManager : MonoSingleton<GameManager>, IDontDestroySingleton
    {
        #region Submanagers
        public static TManager GetSubmanager<TManager>() where TManager : Manager
        {
            GameManager gm = GameManager.Instance;
            if (gm._managers.TryGetValue(typeof(TManager), out Manager result))
            {
                return result as TManager;
            }
            else
            {
                Debug.LogError($"[{nameof(GameManager)}] {typeof(TManager).Name} 매니저가 존재하지 않습니다.");
                return null;
            }
        }
        public static async UniTask<TManager> GetSubmanagerAsync<TManager>() where TManager : Manager
        {
            if (GameManager.Instance && GameManager.Instance._initialized)
            {
                return GetSubmanager<TManager>();
            }
            else
            {
                await UniTask.WaitUntil(() => GameManager.Instance && GameManager.Instance._initialized);
                return GetSubmanager<TManager>();
            }
        }

        private Dictionary<Type, Manager> _managers;
        private bool _initialized;

        protected override void OnInitializeSingleton()
        {
            _managers = new Dictionary<Type, Manager>();

            AddManager<ConfigManager>();
            AddManager<SaveManager>();
            AddManager<LevelManager>();
        }
        private void Start()
        {
            _initialized = true;

            foreach (var manager in _managers.Values)
            {
                manager.Initialize();
            }
        }

        internal void AddManager<TManager>() where TManager : Manager, new()
        {
            if (_managers.ContainsKey(typeof(TManager)))
            {
                Debug.LogWarning($"[{nameof(GameManager)}] {typeof(TManager).Name} 매니저는 이미 등록되어있습니다.");
            }
            else
            {
                var managerInst = new TManager();
                _managers.Add(typeof(TManager), managerInst);

                if (_initialized)
                {
                    managerInst.Initialize();
                }
            }
        }
        #endregion
    }
}
