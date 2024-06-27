using UnityEngine;
using UnityEngine.UI;

namespace Knownt
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        public Slider musicSlider;
        public Slider effectSlider;
        public Slider masterSlider;

        public AudioSource musicSource;
        public AudioSource effectsSource;

        private float masterVolume = 1f;

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
        public void Initialize()
        {
            UpdateMusicVolume(SaveManager.SavedData.AudioData.Music_Volume);
            UpdateEffectVolume(SaveManager.SavedData.AudioData.Effect_Volume);
        }

        public void LoadSliders()
        {
            masterSlider.value = SaveManager.SavedData.AudioData.Master_Volume;
            musicSlider.value = SaveManager.SavedData.AudioData.Music_Volume;
            effectSlider.value = SaveManager.SavedData.AudioData.Effect_Volume;
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
            musicSource.Stop();
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
        public void UpdateMasterVolume(float value)
        {
            masterVolume = value;
            SaveManager.SavedData.AudioData.Master_Volume = value;
            SaveManager.SaveConfigFile();
            UpdateMusicVolume(SaveManager.SavedData.AudioData.Music_Volume);
            UpdateEffectVolume(SaveManager.SavedData.AudioData.Effect_Volume);
        }

        /// <summary> Method to update the music volume. </summary>
        /// <param name="value">New volume value [0-1].</param>
        public void UpdateMusicVolume(float value)
        {
            musicSource.volume = value * masterVolume;
            SaveManager.SavedData.AudioData.Music_Volume = value;
            SaveManager.SaveConfigFile();
        }

        /// <summary> Method to update the sound effect volume. </summary>
        /// <param name="value">New volume value [0-1].</param>
        public void UpdateEffectVolume(float value)
        {
            effectsSource.volume = value * masterVolume;
            SaveManager.SavedData.AudioData.Effect_Volume = value;
            SaveManager.SaveConfigFile();
        }
        #endregion
    }
}