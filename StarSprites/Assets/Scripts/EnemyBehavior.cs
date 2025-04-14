using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float patrolSpeed = 10f;
    public float chaseSpeed = 15f;
    public float detectionRange = 30f;
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public Transform pointA, pointB;
    public int maxHealth = 3;

    private Transform player;
    private Vector3 targetPoint;
    private int currentHealth;
    private bool isChasing = false;
    private bool movingToA = true;
    private float attackCooldownTimer = 0f;
    private bool facingRight;

    private Rigidbody2D rb;

    //animator (lis)
    Animator animator;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPoint = pointA.position;
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

        // Detects player distance to initiate chasing
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > detectionRange * 1.5f)
        {
            isChasing = false;
        }

        // Controls chasing behavior
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
        
        if (transform.localScale.x < 0)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

    }

    // Controls patrol behavior
    void Patrol()
    {
        // Bear walks back and forth between two designated spots
        float step = patrolSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, step);

        if (Vector2.Distance(transform.position, targetPoint) < 1f)
        {
            Debug.Log("Moving to next patrol point!");
            movingToA = !movingToA;
            targetPoint = movingToA ? pointA.position : pointB.position;
            Flip();
        }
    }

    // Bear chases player
    void ChasePlayer()
    {
        // The bear follows the player around at increased speed until the player is out of range
        float step = chaseSpeed * Time.deltaTime;
        Vector3 newPosition = Vector2.MoveTowards(transform.position, player.position, step);


        if ((player.position.x < transform.position.x && facingRight) ||
            (player.position.x > transform.position.x && !facingRight))
        {
            Flip();
        }

        transform.position = newPosition;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Player in range!");
            Attack();
        }
    }

    // Attack animation plays and bear damages player every few seconds
    void Attack()
    {
        if (attackCooldownTimer <= 0)
        {
            Debug.Log("Enemy Attacks!");
            animator.SetTrigger("playerContact");
            HealthBarManager.Instance.TakeDamage(25);
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
        Debug.Log("Enemy Died!");
        XPBarBehavior.Instance.GainXP(25);
        Destroy(gameObject);
    }

    // Flip's bear sprite
    void Flip()
    {
        Vector3 scale = transform.localScale;
        if (facingRight)
        {
            scale.x = -3;
        }
        else
        {
            scale.x = 3;
        }

        transform.localScale = scale;
        Debug.Log("Enemy flipped. New scale: " + transform.localScale);
    }

    // Handles collision with player
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
            Debug.Log("I've been hit!");
        }
    }
}
