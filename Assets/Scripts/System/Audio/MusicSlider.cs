using UnityEngine;
using UnityEngine.UI;

namespace Knownt
{
    public class MusicSlider : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.Instance.musicSlider = GetComponent<Slider>();
        }
    }
}
