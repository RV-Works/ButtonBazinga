using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Flash : MonoBehaviour
{
    Image image = null;
    Coroutine coroutine = null;
    private void Awake()
    {
        image = GetComponent<Image>();

    }

    public void startFlash(float secondsForOneFlash, float maxAlpha, Color flashColor)
    {
        image.color = flashColor;
        //ensure blah blah max aplha max 1
        maxAlpha = Mathf.Clamp(maxAlpha, 0, 1);
        if (coroutine != null)
        
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(Flash1(secondsForOneFlash, maxAlpha));


    }
    IEnumerator Flash1(float secondsForOneFlash, float maxAlpha)
    {
        //in
        float FlashInDuration = secondsForOneFlash / 2;
        for (float  t = 0; t < FlashInDuration; t += Time.deltaTime)
        {
            Color colorthisFrame = image.color;
            colorthisFrame.a = Mathf.Lerp(0, maxAlpha, t / FlashInDuration);
            image.color = colorthisFrame;
            yield return null;
        }
        //out
        float FlashOutDuration = secondsForOneFlash / 2;
        for (float t = 0; t < FlashOutDuration; t += Time.deltaTime)
        {
            Color colorthisFrame = image.color;
            colorthisFrame.a = Mathf.Lerp(maxAlpha, 0, t / FlashOutDuration);
            image.color = colorthisFrame;
            yield return null;
     
        }
        image.color = new Color(0, 0, 0, 0);

    }
}
// https://youtu.be/Yw3EoV5I_PE?si=7J1v5HL0j32W50M3