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
    public int maxHealth = 300;

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
    private bool isAttacking = false;


    private Rigidbody2D rb;

    public HealthBarManager healthBarManager; // Reference to the HealthBarManager script

    //animator
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        facingRight = transform.localScale.x <= 0;
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
        if (isAttacking || isPerformingSpecialAttack)
            return; // Don't move while attacking or special attacking

        float step = chaseSpeed * Time.deltaTime;
        Vector3 newPosition = Vector2.MoveTowards(transform.position, player.position, step);

        if ((newPosition.x < transform.position.x && facingRight) ||
            (newPosition.x > transform.position.x && !facingRight))
        {
            Flip();
        }

        transform.position = newPosition;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
        }
    }


    void Attack()
    {
        if (attackCooldownTimer <= 0 && !isPerformingSpecialAttack && !isAttacking)
        {
            if (isEnraged)
            {
                // Randomly choose between charge attack and ground slam in enraged mode
                if (Random.value < 0.5f)
                    StartCoroutine(DoChargeAttack());
                else
                    StartCoroutine(DoGroundSlam());
            }
            else
            {
                Debug.Log("Enemy attacks with a bite!");
                animator.SetTrigger("playerContact");
                healthBarManager.TakeDamage(10);
                StartCoroutine(EndAttackAfterDelay(1f)); // <- wait a short time before chasing again
            }

            attackCooldownTimer = attackCooldown;
        }
    }


    IEnumerator DoChargeAttack()
    {
        isPerformingSpecialAttack = true;
        yield return new WaitForSeconds(0.5f);

        float timer = chargeDuration;
        Vector2 chargeDirection = (player.position - transform.position).normalized;
        while (timer > 0f)
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + chargeDirection, chargeSpeed * Time.deltaTime);
            healthBarManager.TakeDamage(10); // Damage the player during charge
            timer -= Time.deltaTime;
            yield return null;
        }

        isPerformingSpecialAttack = false;
    }

    IEnumerator DoGroundSlam()
    {
        isPerformingSpecialAttack = true;
        yield return new WaitForSeconds(0.5f);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce);
        yield return new WaitForSeconds(0.5f);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -slamForce);
        healthBarManager.TakeDamage(10); // Damage the player during slam
        yield return new WaitForSeconds(0.5f);

        rb.linearVelocity = Vector2.zero;
        isPerformingSpecialAttack = false;
    }

    IEnumerator EndAttackAfterDelay(float delay)
    {
        isAttacking = true;
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Decrement health by damage amount
        Debug.Log($"Boss took {damage} damage, remaining health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died!");
        XPBarBehavior.Instance.GainXP(50);
        Destroy(gameObject);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(5); // Adjust damage per hit as needed
            Destroy(collision.gameObject);
        }
    }
}

