using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenToggle : MonoBehaviour
{
    public void setFullscreen(bool fullscreen)
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

}
