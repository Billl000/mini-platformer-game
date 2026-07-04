using System;
using UnityEngine;

public class Ink : MonoBehaviour
{
    [SerializeField] private float maxInk = 50;
    private float currentInk = 0;

    public event Action<float> OnInkChanged;

    public float GetMaxInk() => maxInk;
    public float GetCurrentInk() => currentInk;

    private void Awake()
    {
        currentInk = 0;
    }

    public void UseInk(float amount)
    {
        if (currentInk >= amount)
        {
            currentInk -= amount;
        }

        OnInkChanged?.Invoke(currentInk / maxInk);
    }

    public void AddInk(float amount)
    {
        if (currentInk + amount > maxInk)
        {
            currentInk = maxInk;
        }
        else
        {
            currentInk += amount;
        }

        OnInkChanged?.Invoke(currentInk / maxInk);

    }

    public void SetMaxInk(float newMaxInk)
    {
        maxInk = newMaxInk;
        currentInk = 0; 
    }
}
