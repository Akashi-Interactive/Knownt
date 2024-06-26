using TMPro;
using UnityEngine;

namespace Knownt
{
    public class CollectableUI : MonoBehaviour
    {
        public static CollectableUI instance;

        public TextMeshProUGUI collectableText;
        [SerializeField] private int maxCollectableCount;

        private int currentCollectableCount;

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
        }
    }
}
