using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _randomBounceAngle = 10f;
    [SerializeField] private float _paddleBounceAngle = 60f;

    private bool _attachedToPaddle = false;
    private Transform _paddle;

    private void Update()
    {
        if (_attachedToPaddle && _paddle != null)
        {
            Vector3 pos = _paddle.position;
            transform.position = new Vector3(pos.x, pos.y + 0.25f, 0f);

            float horizontal = Input.GetAxis("Horizontal");
            if (Mathf.Abs(horizontal) > 0.01f)
                Launch(Random.Range(-45f, 45f));
        }
    }

    public void AttachToPaddle(Transform paddle)
    {
        _paddle = paddle;
        _attachedToPaddle = true;
        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Launch(float angle = 0f)
    {
        if (_attachedToPaddle)
        {
            _attachedToPaddle = false;
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        _rb.linearVelocity = direction.normalized * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 newVelocity = _rb.linearVelocity.normalized;

        if (collision.gameObject.name == "Paddle")
        {
            Transform paddle = collision.transform;
            float paddleWidth = collision.collider.bounds.size.x;
            float hitPoint = (transform.position.x - paddle.position.x) / (paddleWidth / 2f);
            hitPoint = Mathf.Clamp(hitPoint, -1f, 1f);
            float bounceAngle = hitPoint * _paddleBounceAngle;
            newVelocity = Quaternion.Euler(0, 0, bounceAngle) * Vector2.up;
        }
        else
        {
            float randomAngle = Random.Range(-_randomBounceAngle, _randomBounceAngle);
            newVelocity = Quaternion.Euler(0, 0, randomAngle) * newVelocity;
        }

        _rb.linearVelocity = newVelocity.normalized * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "DeathZone")
        {
            BallManager.Instance.UnregisterBall(this);
            Destroy(gameObject);
        }
    }
}