using UnityEngine;
using UnityEngine.SceneManagement;

namespace Knownt
{
    public class GameSceneManager : MonoBehaviour
    {
        public void QuitGame()
        {
            Application.Quit();
        }

        public void LoadGame()
        {
            SceneManager.LoadScene("Game");
            CanvasController.Instance.ShowGameUI();
        }
    }
}
