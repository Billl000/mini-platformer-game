using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private Checkpoint checkpoint;

    void Awake()
    {
        checkpoint = GetComponent<Checkpoint>();
        if (checkpoint == null)
        {
            Debug.LogError("CheckpointTrigger requires a Checkpoint component on the same GameObject.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            RespawnManager.Instance.ActivateCheckpoint(checkpoint);
    }
}