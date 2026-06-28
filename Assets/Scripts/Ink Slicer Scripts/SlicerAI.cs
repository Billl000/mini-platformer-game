using UnityEngine;
using Pathfinding;

public class SlicerAI : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private float checkRadius = 0.2f;

    [Header("Detection")]

    [SerializeField] private float proximityRange = 20f;
    [SerializeField] private float losRange = 10f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Chasing")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float losePlayerTime = 3f;
    [SerializeField] private float pathUpdateInterval = 1.5f; //Path update cooldown
    [SerializeField] private float nextWaypointDistance = 1f; //distance to next waypoint before moving to the next one

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float telegraphDuration = 0.5f; // Duration of the telegraph before attacking
    [SerializeField] private float lungeForce = 10f; // Force applied during the lunge attack

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f; // Force applied when jumping
    [SerializeField] private float jumpHeightThreshold = 0.8f; //the height diff to determine if it jumps
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;


    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Seeker seeker;

    private enum State { Patrolling, Chasing, Attacking }
    private State currentState = State.Patrolling;

    private bool movingRight = true;
    private float losePlayerTimer = 0f;
    private float telegraphTimer = 0f;
    private float attackCooldownTimer = 0f;
    private bool hasTelegraphed = false;

    private Path currentPath;
    private int currentWaypoint = 0;
    private float pathUpdateTimer = 0f;

    private bool isGrounded = false;
    private bool isJumping = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();

        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling: Patrol(); break;
            case State.Chasing: Chase(); break;
            case State.Attacking: Attack(); break;
        }

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    void Patrol()
    {
        bool wallDetected = Physics2D.OverlapCircle(wallCheck.position, checkRadius, LayerMask.GetMask("Ground"));
        bool edgeDetected = !Physics2D.OverlapCircle(edgeCheck.position, checkRadius, LayerMask.GetMask("Ground"));

        if (DetectsPlayer())
        {
            currentState = State.Chasing;
            Debug.Log($"switching state to {State.Chasing}");
            RequestPath();
        }

        if (wallDetected || edgeDetected)
        {
            FlipAxis();
        }

        rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb.linearVelocity.y); //hopefully won't affect any future mechanics
    }

    void Chase()
    {
        if (player == null) return;

        // Periodically recalculate the path (player keeps moving)
        pathUpdateTimer -= Time.deltaTime;
        if (pathUpdateTimer <= 0f)
        {
            RequestPath();
            pathUpdateTimer = pathUpdateInterval;
        }

        FollowPath();

        //lose player, back to patrolling
        if (!DetectsPlayer())
        {
            losePlayerTimer -= Time.deltaTime;
            if (losePlayerTimer <= 0f)
            {
                currentState = State.Patrolling;
                currentPath = null;
            }
        }
        else
        {
            losePlayerTimer = losePlayerTime;
        }

        // Close enough to strike?
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= attackRange && attackCooldownTimer <= 0f)
        {
            currentState = State.Attacking;
            telegraphTimer = telegraphDuration;
            hasTelegraphed = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Attack()
    {
        if (!hasTelegraphed)
        {
            telegraphTimer -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;
            spriteRenderer.color = Color.red;

            if (telegraphTimer <= 0f)
            {
                hasTelegraphed = true;
                spriteRenderer.color = Color.white;

                Vector2 lungeDir = (player.position - transform.position).normalized;
                lungeDir.y = 0f;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(lungeDir * lungeForce, ForceMode2D.Impulse);

                attackCooldownTimer = attackCooldown;
            }
        }
        else
        {
            if (Mathf.Abs(rb.linearVelocity.x) < 0.5f)
                currentState = State.Chasing;
        }
    }

    ///////// A* Pathfinding /////////
    ///
    
    void RequestPath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
        }
    }

    void FollowPath()
    {
        if (currentPath == null) return;

        if (currentWaypoint >= currentPath.vectorPath.Count) return;

        //ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
       
        Vector2 targetPos = currentPath.vectorPath[currentWaypoint];
        float dirX = targetPos.x - rb.position.x;
        float dirY = targetPos.y - rb.position.y;

        bool shouldJump = dirY > jumpHeightThreshold && isGrounded && !isJumping;

        if (shouldJump)
        {
            Vector2 jumpDir = new Vector2(Mathf.Sign(dirX), 1f).normalized;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical velocity before jumping
            rb.AddForce(new Vector2(jumpDir.x * chaseSpeed, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
        else if (!shouldJump)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(dirX) * chaseSpeed, rb.linearVelocity.y);
        }

        if (isGrounded && isJumping)
        {
            isJumping = false;
        }

        FlipTowardsPlayer(dirX);

        float distToWaypoint = Vector2.Distance(rb.position, targetPos);
        if (distToWaypoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    ///////// Utility Methods /////////
    ///
    void FlipAxis()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x = movingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
    void FlipTowardsPlayer(float dirX)
    {
        if (dirX > 0 && !movingRight)
        {
            FlipAxis();
        }
        else if (dirX < 0 && movingRight)
        {
            FlipAxis();
        }
    }

    bool DetectsPlayer()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= proximityRange)
        {
            return true; // Player is within proximity range
        }

        if (losRange > 0f && distanceToPlayer <= losRange)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, losRange, obstacleLayer);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true; // Player is within line of sight
            }
        }

        return false;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, proximityRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, losRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

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
