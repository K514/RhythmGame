using UnityEngine;

namespace K514
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;
        public static T GetInstance {
            get
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<T>();
                }
                return Instance;
            }
        }
    }
}