using UnityEngine;

public class Enemy2Behavior : MonoBehaviour
{
    public GameObject explosionEffect;
    public float diveSpeed = 5f;
    public float maxFallSpeed = -3f;
    public float despawnTime = 5f; // Time until bird despawns if nothing happens

    private Rigidbody2D rb;
    private Transform player;
    public BirdSpawner spawner;
    private bool alreadyExploded = false; // Prevent double triggering

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Start the despawn countdown
        Invoke(nameof(Despawn), despawnTime);
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x, direction.y) * diveSpeed;

            if (rb.linearVelocity.y < maxFallSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alreadyExploded) return;

        if (collision.collider.CompareTag("Player"))
        {
            Explode(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyExploded) return;
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log("Killed by player");
            Explode(true);
        }
    }

    void Explode(bool killedByPlayer)
    {
        alreadyExploded = true;
        CancelInvoke(nameof(Despawn)); // Stop timer if explosion happens

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (killedByPlayer)
        {
            Debug.Log(spawner != null);
            Debug.Log("Birds will stop");
            spawner.OnBirdKilledByPlayer();
            XPBarBehavior.Instance.GainXP(10);
        }
        else
        {
            HealthBarManager.Instance.TakeDamage(25);
        }

        Destroy(gameObject);
    }

    void Despawn()
    {
        if (alreadyExploded) return;

        // Optional: add fade-out or despawn effect here
        Destroy(gameObject);
    }
}
