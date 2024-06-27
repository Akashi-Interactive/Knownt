using UnityEngine;
using UnityEngine.UI;

namespace Knownt
{
    public class MasterSlider : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioManager.Instance.masterSlider = GetComponent<Slider>();
        }
    }
}
