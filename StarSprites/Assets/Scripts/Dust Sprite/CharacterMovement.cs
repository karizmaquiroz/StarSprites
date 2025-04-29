using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public List<LayerMask> groundLayers = new List<LayerMask>();

    public GameObject projectilePrefab;         // Ability 1
    public GameObject spreadProjectilePrefab;   // Ability 2
    public GameObject bigStarPrefab;            // Ability 3
    public Transform firePoint;
    public float projectileSpeed = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private int maxJumps = 2;
    public GameObject shotpoint;
    public bool isFacingRight;

    private bool isStunned = false;
    private int selectedAbility = 1;
    private float shotpointX;

    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shotpointX = shotpoint.transform.localPosition.x;
    }

    void Update()
    {
        if (isStunned)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetFloat("walking", 0f);
            return;
        }

        float moveInput = Input.GetAxis("Horizontal");
        animator.SetFloat("walking", Mathf.Abs(moveInput));
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
        {
            spriteRenderer.flipX = true;
            shotpoint.transform.localPosition = new Vector3(-shotpointX, 0f, 0f);
            isFacingRight = true;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = false;
            shotpoint.transform.localPosition = new Vector3(shotpointX, 0f, 0f);
            isFacingRight = false;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[0]) ||
                     Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[1]);

        if (isGrounded) jumpCount = 0;

        if (rb.linearVelocity.y == 0 && isGrounded)
            animator.SetTrigger("isGrounded");

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
            animator.SetTrigger("jumping");
        }

        rb.gravityScale = rb.linearVelocity.y > 0 ? 2f : 4f;

        // Ability Selection (keys 1-3)
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedAbility = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedAbility = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedAbility = 3;

        int level = XPBarBehavior.Instance.level;

        if (Input.GetButtonDown("Fire1"))
        {
            switch (selectedAbility)
            {
                case 1:
                    if (level >= 1) Shoot();
                    break;
                case 2:
                    if (level >= 2) SpreadFire();
                    break;
                case 3:
                    if (level >= 3) ExplosiveStar();
                    break;
            }
        }
    }

    public void Stun(float duration)
    {
        if (!isStunned)
            StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        Debug.Log("Player is stunned!");
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(isFacingRight ? projectileSpeed : -projectileSpeed, 0f);
            Destroy(projectile, 5f);
        }
    }

    void SpreadFire()
    {
        float[] angles = { 15f, 0f, -15f };

        foreach (float angle in angles)
        {
            GameObject star = Instantiate(spreadProjectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = star.GetComponent<Rigidbody2D>();

            float direction = isFacingRight ? 1 : -1;
            Vector2 baseDirection = new Vector2(direction, Mathf.Tan(angle * Mathf.Deg2Rad));
            rb.linearVelocity = baseDirection.normalized * projectileSpeed;
            Destroy(star, 5f);
        }
    }

    void ExplosiveStar()
    {
        GameObject bigStar = Instantiate(bigStarPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bigStar.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(isFacingRight ? projectileSpeed : -projectileSpeed, 0f);
        Destroy(bigStar, 5f);
    }
}
