using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath += HandleDeath;
        Debug.Log("Added OnDeath");
    }

    void HandleDeath()
    {
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        health.OnDeath -= HandleDeath;
    }
}
