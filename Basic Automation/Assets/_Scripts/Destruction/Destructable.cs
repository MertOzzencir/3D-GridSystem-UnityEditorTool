using System;
using UnityEngine;

public class Destructable : MonoBehaviour, IInteractable
{
    [SerializeField] private DestructSO data;
    public event Action<Destructable> OnHit;
    private int Health;
    void Start()
    {
        Health = data.Health;
    }
    [ContextMenu("Interact")]

    public void Interact()
    {
        OnHit?.Invoke(this);
    }
    public DestructSO GetData()
    {
        return data;
    }
    public int GetHealth()
    {
        return Health;
    }
    public void SetHealth(int newHealth)
    {
        Health = newHealth;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

