using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float moveSpeed;
    public float jumpForce;

    private static readonly int Running = Animator.StringToHash("running");
    private static readonly int Jumping = Animator.StringToHash("jumping");
    private static readonly int Failing = Animator.StringToHash("falling");

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var velocity = rb.velocity;

        // 玩家移动 & 跳跃
        if (horizontal != 0)
        {
            var horizontalRaw = Math.Sign(horizontal);
            velocity.x = moveSpeed * horizontal * Time.deltaTime;

            transform.localScale = new Vector3(horizontalRaw, 1, 1);
            // 移动动画
            animator.SetFloat(Running, Math.Abs(horizontal));
        }

        var jump = animator.GetBool(Jumping);
        var fall = animator.GetBool(Failing);
        if (!jump && !fall && Input.GetButton("Jump"))
        {
            velocity.y = jumpForce * Time.deltaTime;
            // 跳跃状态切换
            animator.SetBool(Jumping, true);
        }


        if (jump && velocity.y < 0)
        {
            animator.SetBool(Jumping, false);
            animator.SetBool(Failing, true);
        }
        else if (fall && velocity.y == 0)
        {
            animator.SetBool(Failing, false);
        }

        rb.velocity = velocity;
    }
}