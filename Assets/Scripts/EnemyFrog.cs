using UnityEngine;

public class EnemyFrog : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D _rb;

    private float _leftX;
    private float _rightX;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        var left = transform.GetChild(0);
        var right = transform.GetChild(1);

        // 记录边界值
        _leftX = left.position.x;
        _rightX = right.position.x;

        Destroy(left.gameObject);
        Destroy(right.gameObject);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        var posX = _rb.position.x;

        float direction;
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
        var velocity = _rb.velocity;
        velocity.x = direction * moveSpeed * Time.deltaTime;
        _rb.velocity = velocity;
    }
}