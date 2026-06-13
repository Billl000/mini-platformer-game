using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        slider.value = 1f;

        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    void UpdateHealthBar(float health)
    {
        slider.value = health;
    }

    void OnDestroy()
    {
        slider.value = 0f;
        playerHealth.OnHealthChanged -= UpdateHealthBar;
    }
}
