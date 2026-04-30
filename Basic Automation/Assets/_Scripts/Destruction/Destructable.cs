using System;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] private DestructSO data;
    [SerializeField] private string[] animationNames;
   
    private int Health;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        Health = data.Health;
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

