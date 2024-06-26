using UnityEngine;

namespace Knownt
{
    public class PlayAudio : MonoBehaviour
    {        
        public AudioClip audioClip;

        public void StartAudio()
        {
            AudioManager.Instance.PlayOneShot(audioClip);
        }
    }
}