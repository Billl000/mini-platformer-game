using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float damage = 1f;
    private bool isInkAttack = false;
    private Ink playerInk;
    private float inkCost;

    //use hashset to track enemies hit in one swing
    private HashSet<GameObject> enemiesHit = new HashSet<GameObject>();

    public void SetDamage(float newDamage, bool isInkAttack, Ink ink, float cost)
    {
        damage = newDamage;
        this.isInkAttack = isInkAttack;
        playerInk = ink;
        inkCost = cost;
    }

    public void ResetHitList()
    {
        enemiesHit.Clear();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerEnter2D fired | collider: {collision.name} | hitEnemiesThisSwing.Count: {enemiesHit.Count}");
        if (collision.gameObject.tag == "Enemy")
        {
            if (enemiesHit.Contains(collision.gameObject)) return;
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth == null) return;
            
            if (!isInkAttack && enemyHealth.IsImmuneToPlainAttack())
            {
                Debug.Log("Enemy immune to plain attacks");
                return;
            }
            //playerInkStorage.UseInk(inkCostPerAttack);

            if (isInkAttack && playerInk != null)
                playerInk.UseInk(inkCost);
            enemyHealth.TakeDamage(damage);
            enemiesHit.Add(collision.gameObject);
            Debug.Log($"Enemy hit! Damage: {damage}, Enemy Health: {enemyHealth.GetCurrentHealth()}/{enemyHealth.GetMaxHealth()}");
            
        }
    }
}
