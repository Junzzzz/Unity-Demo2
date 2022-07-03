using System;
using UnityEngine;

public class EnemyFrog : Enemy
{
    private static readonly int Jumping = Animator.StringToHash("jumping");
    private static readonly int Falling = Animator.StringToHash("falling");

    public float moveSpeed;
    public float jumpForce;

    private float _leftX;
    private float _rightX;

    // 记录青蛙停止动画播放次数
    private int _idleCount;

    protected override void Start()
    {
        base.Start();

        var left = transform.GetChild(0);
        var right = transform.GetChild(1);

        // 记录边界值
        _leftX = left.position.x;
        _rightX = right.position.x;

        if (_leftX > _rightX)
        {
            (_leftX, _rightX) = (_rightX, _leftX);
        }

        Destroy(left.gameObject);
        Destroy(right.gameObject);

        _idleCount = 0;
    }
    protected override void Movement()
    {
        var isJumping = Animator.GetBool(Jumping);
        var isFalling = Animator.GetBool(Falling);

        // 判断是否停止移动
        if (!isJumping && !isFalling && _idleCount > 0)
        {
            Rb.velocity = new Vector2(0, 0);
            return;
        }

        var posX = Rb.position.x;

        float direction;

        // 朝向
        if (posX <= _leftX)
        {
            direction = 1f;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (_rightX <= posX)
        {
            direction = -1f;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            direction = -transform.localScale.x;
        }

        var velocity = Rb.velocity;
        velocity.x = direction * moveSpeed;

        // 跳跃 降落
        if (isJumping)
        {
            if (velocity.y < 0)
            {
                Animator.SetBool(Falling, true);
                Animator.SetBool(Jumping, false);
            }
        }
        else if (isFalling)
        {
            if (Math.Abs(velocity.y) < 0.1)
            {
                Animator.SetBool(Falling, false);
            }
        }
        else
        {
            velocity.y = jumpForce;
            Animator.SetBool(Jumping, true);
            RandomIdleCount();
        }

        Rb.velocity = velocity;
    }

    private void CompleteAnimationLoop()
    {
        if (_idleCount > 0)
        {
            _idleCount--;
        }
    }

    private void RandomIdleCount()
    {
        _idleCount = UnityEngine.Random.Range(2, 5);
    }
}