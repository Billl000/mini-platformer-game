using UnityEngine;

public class InkBulbBehaviour : MonoBehaviour
{
    [SerializeField] private float maxInkCapacity = 20f;
    private float currentInkCapacity = 0f;
    private float amountToGive = 5f;

    private bool playerClose = false;

    [SerializeField] private Ink playerInk;

    private void Awake()
    {
        currentInkCapacity = maxInkCapacity;
    }

    void Update()
    {
        if (playerClose && Input.GetKeyDown(KeyCode.E))
        {
            PlayerStabbed();
        }
    }


    private void PlayerStabbed()
    {
        if (currentInkCapacity <= 0) return;

        float inkToGive = Mathf.Min(amountToGive, currentInkCapacity);
        currentInkCapacity -= amountToGive;
        playerInk.AddInk(amountToGive);

        Debug.Log("Player Got Ink!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            playerClose = false;
        }
    }
}
