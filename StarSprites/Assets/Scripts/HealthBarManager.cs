using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Image[] hearts;  // Assign in Inspector
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Slider healthBar;
    
    private int health = 100;
    private int maxHealth = 100;
    private int heartsRemaining;

    void Start()
    {
        heartsRemaining = hearts.Length;
        healthBar.maxValue = maxHealth;
        healthBar.value = health; // Start with full health
        UpdateHearts();
    }

    void TakeDamage(int damage)
    {
        if (heartsRemaining <= 0) return;
        
        health -= damage;
        healthBar.value = health;
        
        if (health <= 0)
        {
            RemoveHeart();
        }
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
    
    void CollisionEnter2D(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(25);
        }
    }
    
}