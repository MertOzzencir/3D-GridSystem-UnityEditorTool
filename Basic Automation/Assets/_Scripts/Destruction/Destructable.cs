using System;
using UnityEngine;

public class Destructable : MonoBehaviour, IInteractable
{
    [SerializeField] private DestructSO data;
    [SerializeField] private string[] animationNames;
    public event Action<Destructable> OnHit;
    private int Health;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
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
    public Animator GetAnimator()
    {
        return anim;
    }
    public string CurrentAnimationName(int index)
    {
        return animationNames[index];
    }

}

