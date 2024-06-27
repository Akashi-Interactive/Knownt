using UnityEngine;

namespace Knownt
{
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance;

        [Header("Canvas Config")]
        [SerializeField] private GameObject GameUI;
        [SerializeField] private GameObject GameOverUI;
        [SerializeField] private GameObject PauseUI;
        [SerializeField] private GameObject WinUI;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            ShowGameUI();
        }

        public void ShowGameUI()
        {
            PauseController.Resume();
            DisableAll();
            GameUI.SetActive(true);
        }

        public void ShowGameOverUI()
        {
            DisableAll();
            GameOverUI.SetActive(true);
        }

        public void ShowPauseUI()
        {
            DisableAll();
            PauseUI.SetActive(true);
        }

        public void ShowWinUI()
        {
            DisableAll();
            WinUI.SetActive(true);
        }

        public void DisableAll()
        {
            GameUI.SetActive(false);
            GameOverUI.SetActive(false);
            PauseUI.SetActive(false);
            WinUI.SetActive(false);
        }
    }
}
