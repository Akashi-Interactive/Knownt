using UnityEngine;

namespace Knownt
{
    public class ExitGame : MonoBehaviour
    {
        public void ExitG()
        {
            GameSceneManager.Instance.QuitGame();
        }
    }
}
