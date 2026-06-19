using UnityEngine;

public class InkPuddle : MonoBehaviour
{
    [SerializeField] private int damagePerTick = 1;
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private float puddleDuration = 3f;

    private float tickTimer = 0f;

    private void Start()
    {
        Destroy(gameObject, puddleDuration); //destroy puddle after duration
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        tickTimer -= Time.deltaTime;
        if (tickTimer < 0f)
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerTick);
            }
            tickTimer = tickInterval; //reset timer
        }
    }
}
