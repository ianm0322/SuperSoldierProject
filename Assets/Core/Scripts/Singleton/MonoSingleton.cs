using UnityEngine;

namespace PrSuperSoldier
{
    public interface IDontDestroySingleton { }

    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance => s_Instance;
        public static bool Initialized => s_Initialized;

        private static T s_Instance;
        private static bool s_Initialized;

        private void Awake()
        {
            if (s_Instance == null)
            {
                s_Instance = this as T;
                s_Initialized = true;

                if (this is IDontDestroySingleton)
                {
                    DontDestroyOnLoad(gameObject);
                }
                OnInitializeSingleton();
            }
            else if (!ReferenceEquals(s_Instance, this))
            {
                Destroy(gameObject);
            }
        }
        protected virtual void OnInitializeSingleton() { }
    }
}
