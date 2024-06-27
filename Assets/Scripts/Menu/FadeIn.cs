using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{

    public  CanvasGroup fadeInOutImage;
    public float duracion = 1.0f;
    private bool fadeOutOnStart = true;

    public void Start()
    {
        if (fadeOutOnStart)
        {
            fadeInOutImage.alpha = 1; 
            FadeOut();
        }
    }

    public void FadeInEffect()
    {
        Debug.Log("se esta haciendo fade in");
        StartCoroutine(FadeCanvasGroup(fadeInOutImage, fadeInOutImage.alpha, 1, duracion));
    }

    public void FadeOut()
    {
        Debug.Log("se esta haciendo fade out");

        StartCoroutine(FadeCanvasGroup(fadeInOutImage, fadeInOutImage.alpha, 0, duracion));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duracion)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duracion)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / duracion);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }


}
