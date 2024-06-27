using UnityEngine;

namespace Knownt
{
    public static class PauseController
    {
        public static bool IsPaused { get; private set; }

        public static void Pause()
        {
            Time.timeScale = 0;
            IsPaused = true;
        }

        public static void Resume()
        {
            Time.timeScale = 1;
            IsPaused = false;
        }
    }
}
