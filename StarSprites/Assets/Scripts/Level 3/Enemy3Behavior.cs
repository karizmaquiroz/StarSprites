using System.Collections;
using UnityEngine;

public class Enemy3Behavior : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float patrolRange = 5f;
    public float detectionRange = 6f;
    public float attackRange = 1.5f;
    public float stunDuration = 1f;
    public float retreatDistance = 3f;
    public float cooldownTime = 2f;
    public Transform player;
    public CharacterMovement playerController;

    private Vector2 patrolStart;
    private bool movingRight = true;
    private bool isAttacking = false;
    private float cooldownTimer = 0f;

    private Rigidbody2D rb;

    private enum State { Patrolling, Chasing, Attacking, Retreating, Cooldown }
    private State currentState = State.Patrolling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolStart = transform.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                if (distanceToPlayer < detectionRange)
                    currentState = State.Chasing;
                break;

            case State.Chasing:
                ChasePlayer();
                if (distanceToPlayer < attackRange)
                    StartCoroutine(AttackRoutine());
                break;

            case State.Cooldown:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                    currentState = State.Patrolling;
                break;
        }
    }

    void Patrol()
    {
        float move = movingRight ? patrolSpeed : -patrolSpeed;
        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);

        if (Vector2.Distance(transform.position, patrolStart) > patrolRange)
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);
        if ((direction.x > 0 && !movingRight) || (direction.x < 0 && movingRight))
            Flip();
    }

    IEnumerator AttackRoutine()
    {
        if (isAttacking) yield break;
        isAttacking = true;
        currentState = State.Attacking;

        rb.linearVelocity = Vector2.zero;

        // 1. Stun the player
        CharacterMovement playerController = player.GetComponent<CharacterMovement>();
        if (playerController != null)
            playerController.Stun(stunDuration);

        yield return new WaitForSeconds(stunDuration);

        // 2. Attack three times
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Enemy attack #" + (i + 1));
            // play animation or trigger effect here
            yield return new WaitForSeconds(0.5f); // time between attacks
        }

        // 3. Retreat
        currentState = State.Retreating;
        float retreatDirection = transform.position.x < player.position.x ? -1 : 1;
        float retreatEnd = transform.position.x + retreatDirection * retreatDistance;

        float retreatTime = 1.0f;
        float elapsed = 0f;

        while (elapsed < retreatTime)
        {
            rb.linearVelocity = new Vector2(retreatDirection * patrolSpeed, rb.linearVelocity.y);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        currentState = State.Cooldown;
        cooldownTimer = cooldownTime;
        isAttacking = false;
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
