using UnityEngine;

namespace Knownt
{
    public class PlayGame : MonoBehaviour
    {
        public void PlayG()
        {
            GameSceneManager.Instance.LoadGame();
        }
    }
}
