using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class StartGame : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Full-screen Image used for fade effect (alpha starts at 1)")]
    public Image fadeImage;

    [Header("Timing Settings")]
    [Tooltip("Duration of the fade-in transition in seconds")]
    public float fadeDuration = 1f;

    private void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image is not assigned on " + name);
            return;
        }

        // Ensure the image is visible and fully opaque at start
        fadeImage.gameObject.SetActive(true);
        Color col = fadeImage.color;
        col.a = 1f;
        fadeImage.color = col;

        // Begin fade-in coroutine
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float timer = 0f;
        Color col = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            col.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = col;
            yield return null;
        }

        // Ensure fully transparent and then disable
        col.a = 0f;
        fadeImage.color = col;
        fadeImage.gameObject.SetActive(false);
    }
}
