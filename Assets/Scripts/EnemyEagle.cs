using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagle : Enemy
{
    public float moveSpeed;

    private Rigidbody2D _rb;
    private float _topX;
    private float _bottomX;
    private int _direction = 1;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        
        var top = transform.GetChild(0);
        var bottom = transform.GetChild(1);

        // 记录边界值
        _topX = top.position.y;
        _bottomX = bottom.position.y;

        if (_bottomX > _topX)
        {
            (_topX, _bottomX) = (_bottomX, _topX);
        }

        Destroy(top.gameObject);
        Destroy(bottom.gameObject);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        var posY = _rb.position.y;

        if (posY > _topX)
        {
            _direction = -1;
        }
        else if (posY < _bottomX)
        {
            _direction = 1;
        }

        var velocity = _rb.velocity;
        velocity.y = _direction * moveSpeed;
        _rb.velocity = velocity;
    }
}