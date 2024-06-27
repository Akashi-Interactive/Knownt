using UnityEngine;
using UnityEngine.UI;

public class ButtonAudioClipsHolder : MonoBehaviour
{
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioSource audioSource;

    public void PlayHover()
    {
        audioSource.PlayOneShot(hoverClip);
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickClip);
    }

}
