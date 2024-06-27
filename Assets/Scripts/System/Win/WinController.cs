using UnityEngine;

namespace Knownt
{
    public class WinController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if(CollectableUI.instance.currentCollectableCount >= CollectableUI.instance.maxCollectableCount)
                {                
                    Debug.Log("<color=#1FA0E6>Player wins!</color>");
                    CanvasController.Instance.ShowWinUI();
                    PauseController.Pause();
                }
            }
        }
    }
}
