using UnityEngine;
using UnityEngine.SceneManagement;

namespace Knownt
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        public void QuitGame()
        {
            Application.Quit();
        }

        public void LoadGame()
        {
            SceneManager.LoadScene("Game");
            CanvasController.Instance.ShowGameUI();
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("Menu");
            CanvasController.Instance.DisableAll();
            PauseController.Resume();
        }
    }
}
