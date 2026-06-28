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
        currentInk = maxInk;
    }

    public void UseInk(float amount)
    {
        if (currentInk > amount)
        {
            currentInk -= amount;
        }
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

    }

    public void SetMaxInk(float newMaxInk)
    {
        maxInk = newMaxInk;
        currentInk = 0; 
    }
}
