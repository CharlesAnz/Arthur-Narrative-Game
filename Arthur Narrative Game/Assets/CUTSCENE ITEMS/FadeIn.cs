using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [Header("Background image to be faded in/out")]
    public Image blackFade;
    [Header("How long to wait until starting to fade in/out")]
    public float timeDelay = 1;
    [Header("How long it will take to finish fading in/out")]
    public float timeToCompleteFade = 2;

    private void Start()
    {
        /* FADE IMAGE IN */
        //blackFade.canvasRenderer.SetAlpha(0.0f);
        //fadeIn();

        /* FADE IMAGE OUT */
        blackFade.canvasRenderer.SetAlpha(1.0f);
        //fadeOut();
        StartCoroutine(InitialDelay(timeDelay));
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

    private IEnumerator InitialDelay(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        blackFade.CrossFadeAlpha(0, timeToCompleteFade, false);
    }

}
