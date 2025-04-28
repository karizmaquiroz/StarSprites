using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Level3WardTrigger : MonoBehaviour
{
    [Header("World Sprites")]
    [Tooltip("The SpriteRenderer that displays the ward in the world")]
    public SpriteRenderer wardSpriteRenderer;
    [Tooltip("Sprite to show first (found ward piece)")]
    public Sprite pickupSprite;
    [Tooltip("Sprite to show second (finished ward)")]
    public Sprite completedWardSprite;


    [Header("UI Elements")]
    [Tooltip("UI panel or popup GameObject (inactive by default)")]
    public GameObject popupUI;
    [Tooltip("Full-screen fade image (can be left empty if none)")]
    public Image fadeImage;

    [Header("Scene Settings")]
    [Tooltip("Build index of next scene to load")]
    public int nextSceneIndex;

    [Header("Timing Settings")]
    [Tooltip("How long to show the pickup sprite before swapping to completed ward")]
    public float pickupDuration = 2f;
    [Tooltip("How long to show completed ward before fading out")]
    public float completedDuration = 2f;
    [Tooltip("Duration of the fade-out")]
    public float fadeDuration = 1f;

    [Header("Ward Manager")]
    public WardManager wardManager;
    public int levelIndex;

    private bool triggered = false;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        if (wardSpriteRenderer != null)
            wardSpriteRenderer.enabled = false;

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
            StartCoroutine(HandleWardSequence());
        }
    }

    private IEnumerator HandleWardSequence()
    {
        // Show popup
        if (popupUI != null)
            popupUI.SetActive(true);

        // Show the pickup sprite
        if (wardSpriteRenderer != null && pickupSprite != null)
        {
            wardSpriteRenderer.enabled = true;
            wardSpriteRenderer.sprite = pickupSprite;
        }

        // Wait for pickup sprite duration
        yield return new WaitForSeconds(pickupDuration);

        // Switch to completed ward sprite
        if (wardSpriteRenderer != null && completedWardSprite != null)
        {
            wardSpriteRenderer.sprite = completedWardSprite;
        }

        // Wait for completed ward duration
        yield return new WaitForSeconds(completedDuration);

        // Begin fade out
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;

            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                c.a = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        // Complete level logic
        if (wardManager != null)
            wardManager.CompleteLevel(levelIndex);

        // Load next scene
        if (nextSceneIndex >= 0)
            SceneManager.LoadScene(nextSceneIndex);
    }
}

