using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager Instance;

    public Image[] hearts;  // Assign in Inspector
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Slider healthBar;
    
    private int health = 100;
    private int maxHealth = 100;
    private int heartsRemaining;

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
        heartsRemaining = hearts.Length;
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
        }
        else if (heartsRemaining == 0)
        {
             Debug.Log("No health");
        }
    }
    
    void UpdateHearts()
    {
       for (int i = 0; i < hearts.Length; i++)
       {
            hearts[i].sprite = (i < heartsRemaining) ? fullHeart : emptyHeart;
       }
    }
}