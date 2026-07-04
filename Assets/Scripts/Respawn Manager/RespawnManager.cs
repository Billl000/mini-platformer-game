using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    private Checkpoint currentCheckpoint;
    private Vector3 startPos; // scene start position

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void SetStartPosition(Vector3 position)
    {
        startPos = position;
    }

    public void ActivateCheckpoint(Checkpoint checkpoint)
    {
        if (currentCheckpoint != null && currentCheckpoint != checkpoint)
        {
            currentCheckpoint.Deactivate();
        }

        currentCheckpoint = checkpoint;
        currentCheckpoint.Activate();
    }

    public Vector3 GetRespawnPosition()
    {
        if (currentCheckpoint != null)
        {
            return currentCheckpoint.RespawnPosition;
        }
        else
        {
            return startPos; // Return the scene start position if no checkpoint is activated
        }
    }
}
