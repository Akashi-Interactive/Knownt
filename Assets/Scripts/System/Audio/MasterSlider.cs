using UnityEngine;
using UnityEngine.UI;

namespace Knownt
{
    public class MasterSlider : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.Instance.masterSlider = GetComponent<Slider>();
        }
    }
}
