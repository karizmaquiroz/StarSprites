using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damageAmount = 25; // Amount of damage per spike hit
    public float bounceForce = 12f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Deal damage via HealthBarManager
            if (HealthBarManager.Instance != null)
            {
                HealthBarManager.Instance.TakeDamage(damageAmount);
            }

            // Apply upward bounce to player
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset fall speed
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}