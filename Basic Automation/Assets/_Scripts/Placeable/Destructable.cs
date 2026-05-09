using System;
using System.Collections;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] private DestructSO data;
    [SerializeField] private string[] animationNames;
    [SerializeField] private float deathTimer;
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
    public void OnDeath()
    {
        transform.parent = null;
        StartCoroutine(DeathAnimation());
    }
    private IEnumerator DeathAnimation()
    {
        float duration = deathTimer;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }
        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }

}

