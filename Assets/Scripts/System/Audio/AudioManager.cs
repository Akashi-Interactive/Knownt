using UnityEngine;

namespace Knownt
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        public AudioSource musicSource;
        public AudioSource effectsSource;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        #region Initialize Methods
        /// <summary> Initialize required componets. </summary>
        private void Initialize()
        {
            UpdateMusicVolume(0.7f);
            UpdateEffectVolume(0.7f);
        }
        #endregion

        #region Play Methods
        /// <summary> Play one sound effect </summary>
        /// <param name="audio">The audio clip to play.</param>
        public void PlayOneShot(AudioClip audio)
        {
            effectsSource.PlayOneShot(audio);
        }

        /// <summary> Play music </summary>
        /// <param name="music">The audio clip to play</param>
        public void PlayMusic(AudioClip music)
        {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.Play();
        }
        #endregion

        #region Stop Methods
        /// <summary> Stop the music audio.</summary>
        public void StopMusic()
        {
            musicSource.Stop();
        }

        /// <summary> Pause the music audio. </summary>
        public void PauseMusic()
        {
            musicSource.Pause();
        }

        /// <summary> Resume the music audio. </summary>
        public void ResumeMusic()
        {
            musicSource.Play();
        }
        #endregion
 
        #region Update Volume Methods
        /// <summary> Method to update the music volume. </summary>
        /// <param name="value">New volume value [0-1].</param>
        public void UpdateMusicVolume(float value)
        {
            musicSource.volume = value;
        }

        /// <summary> Method to update the sound effect volume. </summary>
        /// <param name="value">New volume value [0-1].</param>
        public void UpdateEffectVolume(float value)
        {
            effectsSource.volume = value;
        }
        #endregion
    }
}