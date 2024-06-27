using System.Collections;
using UnityEngine;

/// <summary> Allocates all UI scripts. </summary>
namespace DreamKeeper.Assets.Scripts.UI
{
    /// <summary> Class <c>FadeCanvasTransition</c> allows to start a FadeIn / FadeOut Canvas Transition. </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeCanvasTransition: MonoBehaviour
    {
        public delegate void FadeEvent();
        public event FadeEvent OnFadeEnd;

        [Header("Fade Configuration")]
        [SerializeField] private double fadeDuration = 0.45f;

        private CanvasGroup canvasGroup;
        private IEnumerator fadeCoroutine;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        #region Fades Call Methods
        /// <summary> Call the FadeIn transition. </summary>
        public void StartFadeIn()
        {
            if(!gameObject.activeSelf)
                gameObject.SetActive(true);

            if(fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = FadeIn();
            StartCoroutine(fadeCoroutine);
        }

        /// <summary> Call the FadeOut transition </summary>
        /// <param name="shouldModifyGameObject">If the gameobject requires deactivation (default True)</param>
        public void StartFadeOut(bool shouldModifyGameObject = true)
        {
            if(fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            if (gameObject.activeSelf)
            {
                fadeCoroutine = FadeOut(shouldModifyGameObject);
                StartCoroutine(fadeCoroutine);
            }
        }

        #endregion

        #region Fade In/Out Methods;
        /// <summary> Coroutine for FadeIn transition. </summary>
        private IEnumerator FadeIn()
        {
            double currentTime = 0f;
            float startAlpha = 0f;

            while (currentTime < fadeDuration)
            {
                currentTime += Time.unscaledTimeAsDouble; // Independiente al TimeScale;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, (float)(currentTime / fadeDuration));
                yield return null;
            }

            canvasGroup.interactable = true;
            OnFadeEnd?.Invoke();
        }

        /// <summary> Coroutine for FadeOut transition. </summary>
        private IEnumerator FadeOut(bool shouldModifyGameObject)
        {
            canvasGroup.interactable = false;

            double currentTime = 0f;
            float startAlpha = 1f;

            while (currentTime < fadeDuration)
            {
                currentTime += Time.unscaledTimeAsDouble; // Independiente al TimeScale;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, (float)(currentTime / fadeDuration));
                yield return null;
            }

            if(shouldModifyGameObject)
                gameObject.SetActive(false);

            OnFadeEnd?.Invoke();
        }

        /// <summary> Instantly Fade In </summary>
        public void InstantFadeIn()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        }

        /// <summary> Instantly Fade Out </summary>
        public void InstantFadeOut()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }
        #endregion
    }
}