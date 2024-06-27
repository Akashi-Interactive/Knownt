using TMPro;
using UnityEngine;

namespace Knownt
{
    public class CollectableUI : MonoBehaviour
    {
        public static CollectableUI instance;

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
        }
    }
}
