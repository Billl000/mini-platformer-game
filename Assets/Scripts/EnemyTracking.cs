using UnityEngine;

//Enemy tracking script developed w help of AI, might implement A* in the future
public class EnemyTracking : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private float checkRadius = 0.2f;

    [Header("Detecting Player")]
    [SerializeField] private float proximityRange = 20f;
    [SerializeField] private float losRange = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Transform player;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float losePlayerTime = 3f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float telegraphDuration = 0.5f; // Duration of the telegraph before attacking
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackKnockbackForce = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private enum State { Patrolling, Chasing, Attacking }
    private State currentState = State.Patrolling;

    private bool movingRight = true;
    private float losePlayerTimer = 0f;
    private float telegraphTimer = 0f; // Timer for telegraphing the attack
    private float attackCooldownTimer = 0f;
    private bool hasTelegraphed = false; // Flag to check if the telegraph has been shown

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        
        float dirX = player.position.x - transform.position.x;
        rb.linearVelocity = new Vector2(Mathf.Sign(dirX) * chaseSpeed, rb.linearVelocity.y); //move towards the player

        FlipTowardsPlayer(dirX);

        if (!DetectsPlayer())
        {
            losePlayerTimer -= Time.deltaTime;
            if (losePlayerTimer <= 0f)
            {
                currentState = State.Patrolling;
                Debug.Log($"switching state to {State.Patrolling}"); //if enemy lost player, back to patrolling
            }
        } else
        {
            losePlayerTimer = losePlayerTime; //reset the timer if player is detected
        }

        //check distance to player for attack
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer  <= attackRange && attackCooldownTimer <= 0f)
        {
            currentState = State.Attacking;
            telegraphTimer = telegraphDuration;
            hasTelegraphed = false;
            rb.linearVelocity = Vector2.zero; // Stop moving when attacking
            Debug.Log($"switching state to {State.Attacking}");
        }
    }

    void Attack()
    {
        if (!hasTelegraphed)
        {
            // Show telegraph (e.g., change color or play animation)
            spriteRenderer.color = Color.red; // Example telegraph by changing color
            hasTelegraphed = true;

            telegraphTimer -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;

            if (telegraphTimer <= 0f)
            {
                hasTelegraphed = true;
                spriteRenderer.color = Color.grey;

                attackCooldownTimer = attackCooldown; //cooldown after attack
            }
            else
            {
                if (Mathf.Abs(rb.linearVelocity.x) < 0.5f)
                {
                    currentState = State.Chasing;
                    Debug.Log("switching state to " + State.Chasing);
                }
            }
        }
    }

    //Utility Methods
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, losRange, obstacleLayer | playerLayer);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true; // Player is within line of sight
            }
        }

        return false;

    }


}
