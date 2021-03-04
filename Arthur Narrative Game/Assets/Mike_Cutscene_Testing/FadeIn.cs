using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image blackFade;
    public float timeToCompleteFade = 2;

    private void Start()
    {
        /* FADE IMAGE IN */
        //blackFade.canvasRenderer.SetAlpha(0.0f);
        //fadeIn();

        /* FADE IMAGE OUT */
        blackFade.canvasRenderer.SetAlpha(1.0f);
        fadeOut();

    }

    void fadeIn()
    {
        // Change image from invisible (0) to visible (1)...2 represents timelength in seconds...bool ignoreTimeScale set to false
        blackFade.CrossFadeAlpha(1, timeToCompleteFade, false);
    }

    void fadeOut()
    {
        // Change image from visible (1) to invisible (0)...2 represents timelength in seconds...bool ignoreTimeScale set to false
        blackFade.CrossFadeAlpha(0, timeToCompleteFade, false);
    }

}
