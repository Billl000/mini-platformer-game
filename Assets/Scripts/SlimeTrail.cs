using UnityEngine;

public class SlimeTrail : MonoBehaviour
{
    [SerializeField] private GameObject inkPuddlePrefab;
    [SerializeField] private float spawnInterval = 1f;
    
    [SerializeField] private Transform groundCheck;

    //private bool isGrounded = false;

    private float spawnTimer = 0f;
    private void Update()
    {
        if (!Physics2D.OverlapCircle(groundCheck.position, .01f, LayerMask.GetMask("Ground"))) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            Vector3 spawnPos = transform.position - new Vector3(0, 0.4f, 0);
            //Debug.Log("Slime: " + transform.position + " | Puddle: " + spawnPos);

            Instantiate(inkPuddlePrefab, transform.position - new Vector3(0, 0.4f, 0), Quaternion.identity);
            spawnTimer = spawnInterval; //reset timer
        }
    }
}
