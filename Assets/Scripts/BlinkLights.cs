using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class BlinkLights : MonoBehaviour
{
    public List<Light2D> lightsToBlink; // Lista de luces a parpadear
    public float blinkDuration = 100.0f; // Duraci�n total del parpadeo
    public float blinkInterval = 0.2f; // Intervalo entre parpadeos
    public float minIntensity = 30.0f; // Intensidad m�nima
    public float maxIntensity = 80.0f; // Intensidad m�xima

    private void Start()
    {
        StartCoroutine(BlinkLightsCoroutine());
    }

    // Corutina para el parpadeo
    public IEnumerator BlinkLightsCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            // Parpadea cada luz aleatoriamente
            foreach (Light2D light in lightsToBlink)
            {
                if (light != null)
                {
                    // Genera una nueva intensidad aleatoria entre los valores m�nimo y m�ximo
                    float targetIntensity = Random.Range(minIntensity, maxIntensity);

                    // Cambia la intensidad de forma suave para mayor efecto
                    StartCoroutine(ChangeLightIntensity(light, targetIntensity, blinkInterval / 2)); // Transici�n suave
                }
            }

            yield return new WaitForSeconds(blinkInterval); // Esperar entre parpadeos
            elapsedTime += blinkInterval;
        }

        // Asegurar que todas las luces tengan la intensidad m�nima al finalizar el parpadeo
        foreach (Light2D light in lightsToBlink)
        {
            if (light != null)
            {
                light.intensity = minIntensity; // Establece la intensidad m�nima
            }
        }
    }

    // Corutina para cambiar la intensidad de forma gradual
    private IEnumerator ChangeLightIntensity(Light2D light, float targetIntensity, float duration)
    {
        float initialIntensity = light.intensity; // Intensidad inicial de la luz
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolaci�n de la intensidad de la luz
            light.intensity = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / duration);

            yield return null; // Continuar en el siguiente frame
            elapsedTime += Time.deltaTime;
        }

        // Asegurar que la intensidad alcanza el valor final
        light.intensity = targetIntensity;
    }
}
