using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{
    public int damageAmount = 25; // Damage dealt to the player
    public float bounceForce = 12f;
    public float flashDuration = 0.15f; // Duration of each flash
    public int flashCount = 3; // Number of flashes before attacking
    public Color warningColor = Color.red;
    public float spikeShootDuration = 0.2f; // Duration the spikes remain "up"
    public Vector3 spikeUpOffset = new Vector3(0, 1f, 0); // How far spikes move up

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalPosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        originalPosition = transform.position;
    }

    private void Update()
    {
        StartCoroutine(SpikeWarningAndAttack(null));
    }

    private IEnumerator SpikeWarningAndAttack(Collider2D player)
    {
        // Flash warning
        for (int i = 0; i < flashCount; i++)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = warningColor;
            yield return new WaitForSeconds(flashDuration);
            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        // "Shoot" spikes up (move or animate)
        transform.position = originalPosition + spikeUpOffset;

        // Damage and bounce the player
        if (player.CompareTag("Player"))
        {
            if (HealthBarManager.Instance != null)
            {
                HealthBarManager.Instance.TakeDamage(damageAmount);
            }
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }

        yield return new WaitForSeconds(spikeShootDuration);

        // Move spikes back down
        transform.position = originalPosition;
    }
}
