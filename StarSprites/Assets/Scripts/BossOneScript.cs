using UnityEngine;
using System.Collections; // Needed for IEnumerator

public class BossOneScript : MonoBehaviour
{
    public float patrolSpeed = 10f;
    public float chaseSpeed = 15f;
    public float detectionRange = 30f;
    public float attackRange = 5f;
    public float attackCooldown = 2f; // Cooldown duration in seconds
    public Transform pointA, pointB;
    public int maxHealth = 3;

    // New variables for special attacks and enraged mode
    public float chargeSpeed = 20f;
    public float chargeDuration = 1f;
    public float jumpForce = 300f;
    public float slamForce = 20f;

    private Transform player;
    private Vector3 targetPoint;
    private int currentHealth;
    private bool isChasing = false;
    private bool movingToA = true;
    private float attackCooldownTimer = 0f;
    private bool facingRight = true;

    private bool isEnraged = false;
    private bool isPerformingSpecialAttack = false;

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

        // Check for enraged mode activation
        if (!isEnraged && currentHealth <= maxHealth / 2)
        {
            isEnraged = true;
            chaseSpeed *= 1.5f;
            patrolSpeed *= 1.2f;
            attackCooldown *= 0.8f; // Faster attacks when enraged
            Debug.Log("Boss is enraged!");
        }

        // Decrement the cooldown timer
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        // Skip normal movement and behavior during special attacks
        if (isPerformingSpecialAttack)
        {
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
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Update facing direction based on scale.x
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
        // Only attack if cooldown has elapsed and not already performing a special attack
        if (attackCooldownTimer <= 0 && !isPerformingSpecialAttack)
        {
            if (isEnraged)
            {
                // Randomly choose between charge attack and ground slam in enraged mode
                int attackType = Random.Range(0, 2); // 0 for charge attack, 1 for ground slam
                if (attackType == 0)
                {
                    StartCoroutine(DoChargeAttack());
                }
                else
                {
                    StartCoroutine(DoGroundSlam());
                }
            }
            else
            {
                // Normal bite attack
                Debug.Log("Enemy attacks with a bite!");
            }
            attackCooldownTimer = attackCooldown;
        }
    }

    IEnumerator DoChargeAttack()
    {
        isPerformingSpecialAttack = true;
        Debug.Log("Boss prepares a charge attack!");
        // Brief telegraph delay before charging
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Boss charges!");
        float timer = chargeDuration;
        // Lock in the direction at the start of the charge
        Vector2 chargeDirection = (player.position - transform.position).normalized;
        while (timer > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + chargeDirection, chargeSpeed * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        isPerformingSpecialAttack = false;
    }

    IEnumerator DoGroundSlam()
    {
        isPerformingSpecialAttack = true;
        Debug.Log("Boss prepares a ground slam!");
        // Simulate a jump upwards
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vertical velocity
        rb.AddForce(new Vector2(0, jumpForce));
        // Wait for the jump to reach its apex
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Boss slams down!");
        // Apply downward force for slam
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -slamForce);
        // Wait for the slam to complete
        yield return new WaitForSeconds(0.5f);
        // Reset vertical velocity after slam
        rb.linearVelocity = Vector2.zero;
        isPerformingSpecialAttack = false;
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
