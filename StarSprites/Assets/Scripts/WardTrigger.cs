using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class WardTrigger : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("UI panel or popup GameObject (inactive by default)")]
    public GameObject popupUI;
    [Tooltip("Full-screen Image used for fade effect (alpha starts at 0)")]
    public Image fadeImage;

    [Header("Scene Settings")]
    [Tooltip("Build index of the next scene to load")]
    public int nextSceneIndex;

    [Header("Timing Settings")]
    [Tooltip("How long the popup stays visible before fading")]
    public float popupDuration = 2f;
    [Tooltip("Duration of the fade transition")]
    public float fadeDuration = 1f;

    private bool triggered = false;

    [Header("Ward Manager")]
    public WardManager wardManager;
    public int levelIndex;

    private void Reset()
    {
        // Ensure the collider is set as a trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        // Make sure fade image is hidden and transparent at start
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        // Ensure popup UI is inactive if assigned
        if (popupUI != null)
            popupUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(HandleWardPickup());
        }
    }

    private IEnumerator HandleWardPickup()
    {
        // Show popup
        if (popupUI != null)
            popupUI.SetActive(true);

        // Wait for the popup to display
        yield return new WaitForSeconds(popupDuration);

        // Begin fade-out
        if (fadeImage != null)
        {
            // Activate fade image and reset alpha
            fadeImage.gameObject.SetActive(true);
            Color col = fadeImage.color;
            col.a = 0f;
            fadeImage.color = col;

            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                col.a = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = col;
                yield return null;
            }
        }

        // complete level logic
        if (wardManager != null)
            wardManager.CompleteLevel(levelIndex);

        // Load next scene
        if (nextSceneIndex >= 0)
            SceneManager.LoadScene(nextSceneIndex);
    }
}
