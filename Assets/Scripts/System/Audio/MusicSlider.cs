using UnityEngine;
using UnityEngine.UI;

namespace Knownt
{
    public class MusicSlider : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioManager.Instance.musicSlider = GetComponent<Slider>();
        }
    }
}
