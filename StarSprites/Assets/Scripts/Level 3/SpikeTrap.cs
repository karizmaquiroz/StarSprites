using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{
    public int damageAmount = 25;
    public float bounceForce = 12f;

    public float flashDuration = 0.15f;
    public int flashCount = 3;
    public Color warningColor = Color.red;

    public float spikeShootDuration = 0.2f;
    public float spikeCycleDelay = 1f; // delay before repeating the cycle

    public Vector3 spikeUpOffset = new Vector3(0, 1f, 0);

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalPosition;

    private bool isSpikeActive = false;
    Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        originalPosition = transform.position;

        // Start the spike cycle
        StartCoroutine(SpikeCycle());
    }

    private IEnumerator SpikeCycle()
    {
        while (true)
        {
            // Flash warning
            animator.SetTrigger("Warning");
            yield return new WaitForSeconds(2);

            // Spike up
            animator.SetTrigger("Active");

            transform.position = originalPosition + spikeUpOffset;
            isSpikeActive = true;

            yield return new WaitForSeconds(spikeShootDuration);

            // Spike down
            transform.position = originalPosition;
            isSpikeActive = false;

            yield return new WaitForSeconds(spikeCycleDelay);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isSpikeActive && collision.CompareTag("Player"))
        {
            // Damage
            if (HealthBarManager.Instance != null)
            {
                HealthBarManager.Instance.TakeDamage(damageAmount);
            }

            // Bounce
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // cancel downward
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}

