using UnityEngine;
using UnityEngine.UI;

namespace Knownt
{
    public class EffectsSlider : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.Instance.effectSlider = GetComponent<Slider>();
        }
    }
}
