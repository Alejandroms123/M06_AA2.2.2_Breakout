using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public enum PowerUpType { ExtraBall, Shotgun, MultiplyBalls }
    public PowerUpType _type;
    [SerializeField] private float _fallSpeed = 2f;

    public void Initialize(PowerUpType type)
    {
        _type = type;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            switch (_type)
            {
                case PowerUpType.ExtraBall: sr.color = Color.red; break;
                case PowerUpType.Shotgun: sr.color = Color.green; break;
                case PowerUpType.MultiplyBalls: sr.color = Color.blue; break;
            }
        }
    }

    private void Update()
    {
        transform.Translate(Vector2.down * _fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Paddle")
        {
            Activate(collision.transform.position);
            Destroy(gameObject);
        }
        else if (collision.name == "DeathZone")
        {
            Destroy(gameObject);
        }
    }

    private void Activate(Vector2 paddlePosition)
    {
        switch (_type)
        {
            case PowerUpType.ExtraBall: BallManager.Instance.SpawnExtraBall(); break;
            case PowerUpType.Shotgun: BallManager.Instance.SpawnShotgunBalls(paddlePosition); break;
            case PowerUpType.MultiplyBalls: BallManager.Instance.MultiplyBalls(); break;
        }
    }
}