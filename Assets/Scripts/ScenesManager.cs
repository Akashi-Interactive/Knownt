using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Knownt
{
    public class ScenesManager : MonoBehaviour
    {
        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void LoadGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void LoadCredits()
        {
            SceneManager.LoadScene("Creditos");
        }
    }
}
