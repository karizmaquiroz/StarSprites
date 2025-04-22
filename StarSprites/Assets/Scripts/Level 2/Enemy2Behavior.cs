using UnityEngine;

public class Enemy2Behavior : MonoBehaviour
{
    public GameObject explosionEffect;
    public float diveSpeed = 5f;
    public float maxFallSpeed = -3f;
    public float despawnTime = 5f;

    private Rigidbody2D rb;
    private Transform player;
<<<<<<< Updated upstream
    private BirdSpawner spawner;
    private bool alreadyExploded = false;
=======
    private bool alreadyExploded = false; // Prevent double triggering
>>>>>>> Stashed changes

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        GameObject spawnerObj = GameObject.Find("BirdSpawner");
        if (spawnerObj != null)
        {
            spawner = spawnerObj.GetComponent<BirdSpawner>();
            Debug.Log("Spawner assigned in bird.");
        }
        else
        {
            Debug.LogWarning("BirdSpawner not found in scene!");
        }

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
<<<<<<< Updated upstream
        if (alreadyExploded) return;
        alreadyExploded = true;

        Debug.Log("Explode called. KilledByPlayer: " + killedByPlayer);

=======
        Debug.Log("Explode called. KilledByPlayer: " + killedByPlayer);
    
>>>>>>> Stashed changes
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (killedByPlayer)
        {
            Debug.Log("Killed by player!");
<<<<<<< Updated upstream

=======
        
>>>>>>> Stashed changes
            if (spawner != null)
            {
                Debug.Log("Calling spawner.OnBirdKilledByPlayer()");
                spawner.OnBirdKilledByPlayer();
            }
            else
            {
                Debug.LogWarning("Spawner is NULL in bird script!");
            }
        }

        Destroy(gameObject);
    }


    void Despawn()
    {
        if (alreadyExploded) return;

        Debug.Log("Bird despawned due to timeout.");
        Destroy(gameObject);
    }
}
