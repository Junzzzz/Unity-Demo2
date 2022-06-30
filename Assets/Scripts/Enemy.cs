using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private static readonly int AnimatorDeath = Animator.StringToHash("death");

    protected Animator Animator;

    protected virtual void Start()
    {
        Animator = GetComponent<Animator>();
    }
    
    public void Death()
    {
        Animator.SetTrigger(AnimatorDeath);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}