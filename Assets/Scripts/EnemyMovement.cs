using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Detection")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private float checkRadius = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool movingRight = true;

    private Vector3 currentScale;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        currentScale = transform.localScale;
    }
    
    // Update is called once per frame
    void Update()
    {
        bool wallDetected = Physics2D.OverlapCircle(wallCheck.position, checkRadius, LayerMask.GetMask("Ground"));
        bool edgeDetected = !Physics2D.OverlapCircle(edgeCheck.position, checkRadius, LayerMask.GetMask("Ground"));

        if (wallDetected || edgeDetected)
        {
            FlipAxis();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb.linearVelocity.y);
    }

    void FlipAxis()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x = movingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
        }
        if (edgeCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(edgeCheck.position, checkRadius);
        }
    }
}
