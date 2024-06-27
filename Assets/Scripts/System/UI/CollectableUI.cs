using TMPro;
using UnityEngine;

namespace Knownt
{
    public class CollectableUI : MonoBehaviour
    {
        public static CollectableUI instance;

        public RectTransform collectableTransform;
        public TextMeshProUGUI collectableText;
        [field: SerializeField] public int maxCollectableCount { get; private set; }

        public int currentCollectableCount { get; private set; }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        public void UpdateCollectableText()
        {
            currentCollectableCount++;
            collectableText.text = currentCollectableCount.ToString() + "/" + maxCollectableCount.ToString();

            if (currentCollectableCount >= maxCollectableCount)
            {
                Debug.Log("<color=#1FA0E6>Player wins!</color>");
                CanvasController.Instance.ShowWinUI();
                PauseController.Pause();
            }
        }
    }
}
