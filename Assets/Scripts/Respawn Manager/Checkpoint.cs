using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color inactiveColor = Color.white;
    [SerializeField] private Color activeColor = Color.blue;

    private bool isActivated = false;

    public Vector3 RespawnPosition => transform.position;

    public void Activate()
    {
        if (isActivated) return;
        isActivated = true;
        spriteRenderer.color = activeColor;
    }

    public void Deactivate()
    {
        isActivated = false;
        spriteRenderer.color = inactiveColor;
    }
}
