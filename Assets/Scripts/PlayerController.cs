using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private static readonly int Running = Animator.StringToHash("running");
    private static readonly int Jumping = Animator.StringToHash("jumping");
    private static readonly int Failing = Animator.StringToHash("falling");
    private static readonly int Crouching = Animator.StringToHash("crouching");
    private static readonly int Hurt = Animator.StringToHash("hurt");

    private Rigidbody2D _rb;
    private BoxCollider2D _box;
    private Animator _animator;

    public LayerMask groundMask;
    public AudioSource bgm, jumpSound, hurtSound, hitSound, collectSound;
    public float moveSpeed;
    public float jumpForce;
    private bool _hurting;
    private float _deadLineY;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

        _deadLineY = transform.childCount > 0 ? transform.GetChild(0).position.y : float.MinValue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_hurting)
        {
            PlayerMove();
        }

        CheckDeath();
    }

    private void PlayerMove()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var velocity = _rb.velocity;

        // 玩家移动
        if (horizontal != 0)
        {
            var horizontalRaw = Math.Sign(horizontal);
            transform.localScale = new Vector3(horizontalRaw, 1, 1);
            velocity.x = moveSpeed * horizontal * Time.fixedDeltaTime;
        }

        // 移动动画
        _animator.SetFloat(Running, Math.Abs(horizontal));

        var jump = _animator.GetBool(Jumping);
        var fall = _animator.GetBool(Failing);
        var crouch = _animator.GetBool(Crouching);
        if (!jump && !fall)
        {
            if (!crouch && Input.GetButton("Jump"))
            {
                velocity.y = jumpForce * Time.fixedDeltaTime;
                // 跳跃状态切换
                jumpSound.Play();
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
            case false when (jump && velocity.y <= 0.02f) || (!jump && velocity.y < -2):
                SwitchCrouch(false);
                _animator.SetBool(Crouching, false);
                _animator.SetBool(Jumping, false);
                _animator.SetBool(Failing, true);
                break;
            case true when velocity.y > -0.02f:
                _animator.SetBool(Failing, false);
                break;
        }

        _rb.velocity = velocity;
    }

    private void CheckDeath()
    {
        if (transform.position.y < _deadLineY)
        {
            Invoke(nameof(ReloadScene), 2f);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            collectSound.Play();
            var collectibles = collision.gameObject.GetComponent<Collectibles>();
            collectibles.Collect();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var target = collision.gameObject;
        if (target.CompareTag("Enemies"))
        {
            var velocity = _rb.velocity;
            if (!_hurting && _animator.GetBool(Failing))
            {
                target.GetComponent<Enemy>().Death();
                // 击败敌人
                hitSound.Play();
                velocity.y = jumpForce * Time.deltaTime;
                // 跳跃
                _animator.SetBool(Failing, false);
                _animator.SetBool(Jumping, true);
            }
            else
            {
                _hurting = true;
                _animator.SetTrigger(Hurt);
                // 玩家受伤
                hurtSound.Play();
                velocity.x = (target.transform.localPosition.x > _rb.position.x ? -1 : 1) * moveSpeed  * Time.deltaTime;
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