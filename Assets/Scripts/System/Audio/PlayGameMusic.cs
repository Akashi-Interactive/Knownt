using UnityEngine;

namespace Knownt
{
    public class PlayGameMusic : MonoBehaviour
    {
        public AudioClip music;
        void Start()
        {
            AudioManager.Instance.PlayMusic(music);
        }
    }
}
