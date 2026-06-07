using UnityEngine;

public class EnemyContact : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 20f;
    [SerializeField] private float knockbackVertical = 20f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player!");

            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                //Debug.Log($"Player took {damage} damage. Current health: {playerHealth.GetCurrentHealth()}");

                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    if (collision.transform.position.y > transform.position.y + 0.8)
                    {
                        // If the player is above the enemy, apply vertical knockback
                        playerController.ApplyKnockback(new Vector2(0, knockbackVertical));
                    }
                    else
                    {
                        Vector2 knockbackDirection = new Vector2(collision.transform.position.x - transform.position.x, 0).normalized;
                        playerController.ApplyKnockback(knockbackDirection * knockbackForce);
                    }
                }
            }
        }
    }
}
