using UnityEngine;
using UnityEngine.UI;

namespace Knownt
{
    public class EffectsSlider : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioManager.Instance.effectSlider = GetComponent<Slider>();
        }
    }
}
