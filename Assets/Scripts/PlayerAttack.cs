using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private GameObject attackHitbox;

    private float attackTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isAttacking = false;

    void Update()
    {
        //if on cooldown
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        //if can attack
        if (Input.GetButtonDown("Fire1") && cooldownTimer <= 0f)
        {
            //Debug.Log("attack");
            StartAttack();
        }

        //if attacking
        if (isAttacking) {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                EndAttack();
            }
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        attackTimer = attackDuration;
        cooldownTimer = attackDuration + 0.5f; // Add a small cooldown after the attack
        attackHitbox.SetActive(true);
    }

    void EndAttack()
    {
        isAttacking = false;
        attackHitbox.SetActive(false);
    }
}
