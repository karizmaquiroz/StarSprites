using UnityEngine;

public class BossOneScript : MonoBehaviour
{
    public float patrolSpeed = 10f;
    public float chaseSpeed = 15f;
    public float detectionRange = 30f;
    public float attackRange = 5f;
    public float attackCooldown = 2f; // Cooldown duration in seconds
    public Transform pointA, pointB;
    public int maxHealth = 3;

    private Transform player;
    private Vector3 targetPoint;
    private int currentHealth;
    private bool isChasing = false;
    private bool movingToA = true;
    private float attackCooldownTimer = 0f;
    private bool facingRight = true; // Track the initial facing direction

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPoint = pointA.position;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player GameObject is tagged as 'Player'.");
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > detectionRange * 1.5f)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            Debug.Log("Chasing Player!");
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Decrement the cooldown timer
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        if (transform.localScale.x > 0)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }

    void Patrol()
    {
        float step = patrolSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, step);

        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            movingToA = !movingToA;
            targetPoint = movingToA ? pointA.position : pointB.position;
            Flip();
        }
    }

    void ChasePlayer()
    {
        float step = chaseSpeed * Time.deltaTime;
        Vector3 newPosition = Vector2.MoveTowards(transform.position, player.position, step);

        // Flip the bear to face the direction it is moving
        if ((newPosition.x < transform.position.x && facingRight) ||
            (newPosition.x > transform.position.x && !facingRight))
        {
            Debug.Log("Flipping bear to face direction: " + (newPosition.x < transform.position.x ? "left" : "right"));
            Flip();
        }

        transform.position = newPosition;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Player in range!");
            Attack();
        }
    }

    void Attack()
    {
        // Check if the cooldown period has elapsed
        if (attackCooldownTimer <= 0)
        {
            // Play attack animation if using Animator
            Debug.Log("Enemy Attacks!");

            // Reset the cooldown timer
            attackCooldownTimer = attackCooldown;
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died!");
        XPBarBehavior.Instance.GainXP(25);
        Destroy(gameObject);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        Debug.Log("Bear flipped. New scale: " + transform.localScale);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
            Debug.Log("I've been hit!");
        }
    }
}
