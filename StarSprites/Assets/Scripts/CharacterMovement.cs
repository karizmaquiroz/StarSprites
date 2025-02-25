using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public List<LayerMask> groundLayers = new List<LayerMask>();

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private int maxJumps = 2;
    public GameObject shotpoint;
    public bool isFacingRight;

   //animator (lis)
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        animator.SetFloat("walking", Mathf.Abs(moveInput));
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
        {
            spriteRenderer.flipX = true;
            shotpoint.transform.localPosition = new Vector3(9.4f, 0f, 0f);
            isFacingRight = true;
            //add SOundEffectManager.Play("PlayerMovement");
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = false;
            shotpoint.transform.localPosition = new Vector3(-9.4f, 0f, 0f);
            isFacingRight = false;
            //add SOundEffectManager.Play("PlayerMovement");
        }

        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[0]) || Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayers[1]))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        Debug.Log(isGrounded);
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (rb.linearVelocityY == 0 && isGrounded)
        {
            animator.SetTrigger("isGrounded");
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
            Debug.Log(jumpCount);
            animator.SetTrigger("jumping");
        }
        if (rb.linearVelocity.y > 0)
        {
            rb.gravityScale = 2f;
        }
        else
        {
            rb.gravityScale = 4f;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (isFacingRight)
            {
                rb.linearVelocity = new Vector2(projectileSpeed, 0f);
            }
            else
            {
                rb.linearVelocity = new Vector2(-projectileSpeed, 0f);
            }
                Destroy(projectile, 5f);
        }
    }
}
