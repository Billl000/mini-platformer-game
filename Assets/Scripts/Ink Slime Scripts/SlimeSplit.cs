using System.Drawing;
using UnityEngine;

public class SlimeSplit : MonoBehaviour
{
    [SerializeField] private GameObject smallerSlimePrefab;
    [SerializeField] private float splitScale = 0.5f;
    [SerializeField] private float splitForce = 5f;
    [SerializeField] private float minScaleSplit = 0.5f;

    private Health health;

    [SerializeField] private float sizeMultiplier = 1f;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath += HandleDeath;
    }
    void HandleDeath()
    {
        if (sizeMultiplier > minScaleSplit)
        {
            SpawnChild(Vector3.left);
            SpawnChild(Vector3.right);
        }

        Destroy(gameObject);
    }

    void SpawnChild(Vector3 direction)
    {
        //gameObject.SetActive(false);
        GameObject child = Instantiate(smallerSlimePrefab, transform.position + direction * 0.5f, Quaternion.identity);
        //child.SetActive(false);

        float newScaleMagnitude = Mathf.Abs(transform.localScale.x) * splitScale;
        child.transform.localScale = new Vector3(newScaleMagnitude, newScaleMagnitude, 1);

        SlimeSplit childSplit = child.GetComponent<SlimeSplit>();
        if (childSplit != null)
            childSplit.sizeMultiplier = sizeMultiplier * splitScale;

        
        //change health accordingly
        Health childHealth = child.GetComponent<Health>();
        if (childHealth != null) {
            childHealth.SetMaxHealth(Mathf.Max(1, Mathf.RoundToInt(health.GetMaxHealth() * splitScale)));
        }

        //child.SetActive(true);

        Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * splitForce, ForceMode2D.Impulse);
        }
    }

    private void OnDestroy()
    {
        health.OnDeath -= HandleDeath;
    }
}
