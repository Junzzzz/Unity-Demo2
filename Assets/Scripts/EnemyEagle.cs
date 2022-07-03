public class EnemyEagle : Enemy
{
    public float moveSpeed;

    private float _topX;
    private float _bottomX;
    private int _direction = 1;

    protected override void Start()
    {
        base.Start();
        
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

    protected override void Movement()
    {
        var posY = Rb.position.y;

        if (posY > _topX)
        {
            _direction = -1;
        }
        else if (posY < _bottomX)
        {
            _direction = 1;
        }

        var velocity = Rb.velocity;
        velocity.y = _direction * moveSpeed;
        Rb.velocity = velocity;
    }
}