using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _velocity = 10f;

    private float _horizontal;

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_horizontal * _velocity, 0f);
    }

    public void ResetPosition()
    {
        _rb.position = new Vector2(0f, _rb.position.y);
        _rb.linearVelocity = Vector2.zero;
    }
}