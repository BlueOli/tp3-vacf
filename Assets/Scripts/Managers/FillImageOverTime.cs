using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillImageOverTime : MonoBehaviour
{
    public Image fillImage;
    public float fillDuration = 8f;

    private Coroutine fillCoroutine;

    public void StartFillCoroutine()
    {
        // Stop the previous coroutine if it's running
        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);

        // Start a new fill coroutine
        fillCoroutine = StartCoroutine(FillImageCoroutine());
    }

    IEnumerator FillImageCoroutine()
    {
        while (true)
        {
            float elapsedTime = 0f;

            // Loop until the fill duration is reached
            while (elapsedTime < fillDuration)
            {
                // Calculate the fill amount based on the elapsed time
                float fillAmount = elapsedTime / fillDuration;
                fillImage.fillAmount = fillAmount;

                // Wait for the next frame
                yield return null;

                // Update the elapsed time
                elapsedTime += Time.deltaTime;
            }

            // Reset the fill amount to 0 before starting the next cycle
            fillImage.fillAmount = 0f;
        }
    }
}
