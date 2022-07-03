using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private static readonly int AnimatorDeath = Animator.StringToHash("death");

    private bool _movable = true;

    protected Rigidbody2D Rb;
    protected Animator Animator;

    protected virtual void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        if (_movable)
        {
            Movement();
        }
    }

    protected abstract void Movement();

    public void Death()
    {
        _movable = false;
        GetComponent<Collider2D>().enabled = false;
        Rb.bodyType = RigidbodyType2D.Static;
        Animator.SetTrigger(AnimatorDeath);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}