using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    //[SerializeField] private int damage = 1;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private GameObject attackHitbox;


    [Header("Damage")]
    [SerializeField] private float plainDamage = 1f;
    [SerializeField] private float inkDamage = 3f;
    [SerializeField] private float inkCostPerAttack = 5f;

    private Ink playerInkStorage;
    private AttackHitbox hitboxScript;

    private float attackTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isAttacking = false;

    

    void Awake()
    {
        playerInkStorage = GetComponent<Ink>();
        hitboxScript = GetComponentInChildren<AttackHitbox>(true);
    }
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

        bool hasEnoughInk = playerInkStorage.GetCurrentInk() >= inkCostPerAttack;
        Debug.Log($"Ink amount: {playerInkStorage.GetCurrentInk()}");

        if (hasEnoughInk)
        {
            hitboxScript.SetDamage(inkDamage, true, playerInkStorage, inkCostPerAttack);
        } else
        {
            hitboxScript.SetDamage(plainDamage, false, playerInkStorage, inkCostPerAttack);
        }

        hitboxScript.ResetHitList();
        attackHitbox.SetActive(true);
    }

    void EndAttack()
    {
        isAttacking = false;
        attackHitbox.SetActive(false);
    }
}
