using UnityEngine;

namespace Knownt
{
    public class SystemManager : MonoBehaviour
    {
        public static SystemManager Instance;

        public static Color playerColor;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }
}