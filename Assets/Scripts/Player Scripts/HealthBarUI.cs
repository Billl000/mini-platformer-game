using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    private Slider slider;
    private TMPro.TextMeshProUGUI healthText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        healthText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        //healthText.text = $"{playerHealth.GetMaxHealth()}/{playerHealth.GetMaxHealth()}";
    }

    void Start()
    {
        slider.value = 1f;

        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    void UpdateHealthBar(float health)
    {
        slider.value = health;

        //healthText.text = $"{playerHealth.GetCurrentHealth()}/{playerHealth.GetMaxHealth()}";
    }

    void OnDestroy()
    {
        slider.value = 0f;
        playerHealth.OnHealthChanged -= UpdateHealthBar;
    }
}
