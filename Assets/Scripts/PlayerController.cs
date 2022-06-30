using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int Running = Animator.StringToHash("running");
    private static readonly int Jumping = Animator.StringToHash("jumping");
    private static readonly int Failing = Animator.StringToHash("falling");
    private static readonly int Crouching = Animator.StringToHash("crouching");
    private static readonly int Hurt = Animator.StringToHash("hurt");

    private ScoreController _scoreController;

    private Rigidbody2D _rb;
    private BoxCollider2D _box;
    private Animator _animator;

    public LayerMask groundMask;
    public float moveSpeed;
    public float jumpForce;
    private bool _hurting;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

        _scoreController = FindObjectOfType<ScoreController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_hurting)
        {
            PlayerMove();
        }
    }

    private void PlayerMove()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var velocity = _rb.velocity;

        // 玩家移动 & 跳跃
        if (horizontal != 0)
        {
            var horizontalRaw = Math.Sign(horizontal);
            velocity.x = moveSpeed * horizontal * Time.deltaTime;

            transform.localScale = new Vector3(horizontalRaw, 1, 1);
            // 移动动画
            _animator.SetFloat(Running, Math.Abs(horizontal));
        }

        var jump = _animator.GetBool(Jumping);
        var fall = _animator.GetBool(Failing);
        var crouch = _animator.GetBool(Crouching);
        if (!jump && !fall)
        {
            if (!crouch && Input.GetButton("Jump"))
            {
                velocity.y = jumpForce * Time.deltaTime;
                // 跳跃状态切换
                _animator.SetBool(Jumping, true);
            }
            else if (Input.GetButton("Crouch"))
            {
                // 降低碰撞体积
                SwitchCrouch(true);

                _animator.SetBool(Crouching, true);
            }
            else if (crouch && CanStandUp())
            {
                // 还原碰撞体积
                SwitchCrouch(false);
                _animator.SetBool(Crouching, false);
            }
        }

        switch (fall)
        {
            case false when velocity.y < 0 && Math.Abs(velocity.y) > 2:
                _animator.SetBool(Jumping, false);
                _animator.SetBool(Failing, true);
                break;
            case true when Math.Abs(velocity.y) < 2:
                _animator.SetBool(Failing, false);
                break;
        }

        _rb.velocity = velocity;
    }

    private bool CanStandUp()
    {
        return !Physics2D.OverlapBox(_box.bounds.center, _box.size, 0, groundMask);
    }

    private void SwitchCrouch(bool isCrouch)
    {
        _box.enabled = !isCrouch;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collections"))
        {
            Destroy(collision.gameObject);
            _scoreController.AddScore(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var target = collision.gameObject;
        if (target.CompareTag("Enemies"))
        {
            var velocity = _rb.velocity;
            if (_animator.GetBool(Failing))
            {
                target.GetComponent<Enemy>().Death();
                velocity.y = jumpForce * Time.deltaTime;
                // 跳跃
                _animator.SetBool(Failing, false);
                _animator.SetBool(Jumping, true);
            }
            else
            {
                _animator.SetTrigger(Hurt);
                _hurting = true;
                velocity.x = (target.transform.localPosition.x > _rb.position.x ? -1 : 1) * moveSpeed * 0.2f *
                             Time.deltaTime;
                velocity.y = jumpForce * 0.2f * Time.deltaTime;
            }

            _rb.velocity = velocity;
        }
    }

    public void EndHurting()
    {
        _hurting = false;
    }
}