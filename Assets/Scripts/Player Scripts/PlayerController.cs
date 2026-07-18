using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 16f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.15f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private float horizontalInput;
    private float coyoteTimeCounter;

    [Header("Dashing")]
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashCooldown = 0f;
    [SerializeField] private float dashCooldownDuration = 1f;
    [SerializeField] private float dashDuration = 0.15f;

    private bool isDashing = false;
    private float dashTimer = 0f;

    private bool isKnockedback;
    private float knockbackTime = 0f;
    private float knockbackDuration = 0.5f;

    [SerializeField] private Transform attackHitbox;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isKnockedback)
        {
            knockbackTime -= Time.deltaTime;
            if (knockbackTime <= 0f)
            {
                isKnockedback = false;
            }
            return; // Skip normal movement while knockedback is active
        }

        horizontalInput = Input.GetAxisRaw("Horizontal"); // -1, 0, or 1

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded) {
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimeCounter = 0f;
        }

        if (dashCooldown > 0f)
            dashCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldown <= 0f)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldown = dashCooldownDuration;

            // Dash in facing direction
            float dashDir = spriteRenderer.flipX ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset Y
            rb.AddForce(new Vector2(dashDir * dashForce, 0f), ForceMode2D.Impulse);

        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
        //i  have sprite 
        if (horizontalInput > 0f)
        {
            spriteRenderer.flipX = false;
            attackHitbox.localPosition = new Vector3(0.6f, attackHitbox.localPosition.y, attackHitbox.localPosition.z);
        }
        else if (horizontalInput < 0f)
        {
            spriteRenderer.flipX = true;
            attackHitbox.localPosition = new Vector3(-0.6f, attackHitbox.localPosition.y, attackHitbox.localPosition.z);
        }


    }

    public void ApplyKnockback(Vector2 force)
    {
        isKnockedback = true;
        knockbackTime = knockbackDuration;
        rb.linearVelocity = Vector2.zero; // Stop current movement before applying knockback
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        if (isKnockedback || isDashing) return;
      
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }
}