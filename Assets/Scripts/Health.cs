using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;

    public event Action OnDeath;
    public event Action<float> OnHealthChanged; // Pass current health as parameter

    [SerializeField] private bool immuneToPlainAttack = false;
    public float GetMaxHealth() => maxHealth;
    public float GetCurrentHealth() => currentHealth;

    public bool IsImmuneToPlainAttack() => immuneToPlainAttack;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnHealthChanged?.Invoke(currentHealth / maxHealth);
        if (currentHealth <= 0f)
        {
            OnDeath.Invoke();
            Debug.Log($"{gameObject.name} has died.");
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(1f);
    }

    public void SetMaxHealth(int newMax)
    {
        maxHealth = newMax;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
}
