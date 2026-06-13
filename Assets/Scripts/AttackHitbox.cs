using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Enemy hit! Damage: {damage}, Enemy Health: {enemyHealth.GetCurrentHealth()}/{enemyHealth.GetMaxHealth()}");
            }
        }
    }
}
