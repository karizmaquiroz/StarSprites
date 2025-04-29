using UnityEngine;

public class BossEnemy1Behavior : MonoBehaviour
{
    public float chaseSpeed = 15f;
    public float detectionRange = 30f;
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public int maxHealth = 3;

    private Transform player;
    private int currentHealth;
    private float attackCooldownTimer = 0f;
    private bool facingRight;

    private Rigidbody2D rb;

    // Animator (lis)
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        // Checks which direction bear is facing
        if (transform.localScale.x < 0)
        {
            facingRight = false;
            Flip();
        }
        else
        {
            facingRight = true;
            Flip();
        }
    }

    void Update()
    {
        // Checks if player exists
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player GameObject is tagged as 'Player'.");
            return;
        }

        // Distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Always chase the player if within detection range
        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop moving if player is out of range
        }

        // Decrement the cooldown timer
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        // Update facing direction
        if (transform.localScale.x < 0)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }

    // Bear chases player
    void ChasePlayer()
    {
        float step = chaseSpeed * Time.deltaTime;
        Vector3 newPosition = Vector2.MoveTowards(transform.position, player.position, step);

        // Flip sprite if necessary
        if ((player.position.x < transform.position.x && facingRight) ||
            (player.position.x > transform.position.x && !facingRight))
        {
            Flip();
        }

        transform.position = newPosition;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
        }
    }

    // Attack animation plays and bear damages player every few seconds
    void Attack()
    {
        if (attackCooldownTimer <= 0)
        {
            animator.SetTrigger("playerContact");
            HealthBarManager.Instance.TakeDamage(5);
            attackCooldownTimer = attackCooldown;
        }
    }

    // Allows the bear to take damage
    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Allows bear to die
    void Die()
    {
        XPBarBehavior.Instance.GainXP(34);
        Destroy(gameObject);
    }

    // Flip's bear sprite
    void Flip()
    {
        Vector3 scale = transform.localScale;
        if (facingRight)
        {
            scale.x *= -1;
        }
        else
        {
            scale.x *= -1;
        }

        transform.localScale = scale;
    }

    // Handles collision with bullet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }
}
