using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScene : MonoBehaviour
{
    public GameObject pantalla1;

    public FadeIn fadeIn;

    void Start()
    {
        Invoke("activatePantalla2", 4f);
    }

    public void activatePantalla2()
    {
        Invoke("startFadeIn", 4f);
        pantalla1.SetActive(false);
        Invoke("LoadMainMenu", 1f);
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void startFadeIn()
    {
        fadeIn.FadeInEffect();
    }


}