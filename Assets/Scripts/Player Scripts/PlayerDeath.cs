using UnityEngine;
using System.Collections; // For IEnumerator

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 2f;

    private Health health;
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Vector3 respawnPoint;

    void Awake()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        RespawnManager.Instance.SetStartPosition(transform.position); // Set initial respawn point to starting position

        Debug.Log(respawnPoint);

        health.OnDeath += HandleDeath; // Subscribe to death event
    }

    void HandleDeath()
    {
        StartCoroutine(Respawn()); // respawn process
    }

    IEnumerator Respawn()
    {
        playerController.enabled = false;
        rb.linearVelocity = Vector3.zero;
        rb.simulated = false; // off physics

        yield return new WaitForSeconds(respawnDelay);

        Vector3 respawnPosition = RespawnManager.Instance.GetRespawnPosition();
        transform.position = respawnPosition;
        health.ResetHealth();

        rb.simulated = true; // on physics
        rb.linearVelocity = Vector3.zero; //remove residual forces
        playerController.enabled = true;
    }

    void OnDestroy()
    {
        health.OnDeath -= HandleDeath;   
    }
}
