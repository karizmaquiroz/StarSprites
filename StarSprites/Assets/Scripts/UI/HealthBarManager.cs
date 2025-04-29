using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // <-- Needed to reload scene
using System.Collections;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager Instance;

    public Image[] hearts;  // Assign in Inspector
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Slider healthBar;

    public int health = 100;
    private int maxHealth = 100;
    public int heartsRemaining;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        heartsRemaining = 3;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth; // Start with full health
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        if (heartsRemaining <= 0) return;

        health -= damage;
        healthBar.value = health;

        if (health <= 0)
        {
            RemoveHeart();
        }
        Debug.Log(healthBar.value);
    }

    void RemoveHeart()
    {
        if (heartsRemaining > 0)
        {
            heartsRemaining--;
            hearts[heartsRemaining].sprite = emptyHeart;
            health = maxHealth;
            healthBar.value = health;
            UpdateHearts();
        }

        if (heartsRemaining <= 0)
        {
            Debug.Log("No health");
            Die();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < heartsRemaining) ? fullHeart : emptyHeart;
        }
    }

    void Die()
    {
        Debug.Log("Player died. Restarting scene...");
        StartCoroutine(RestartScene());
    }

    IEnumerator RestartScene()
    {
        // Optional small delay so death feels a bit nicer
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
