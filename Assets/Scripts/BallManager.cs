using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance;
    [SerializeField] private Ball _ballPrefab;
    private List<Ball> _balls = new List<Ball>();

    private void Awake() => Instance = this;

    public void RegisterBall(Ball ball)
    {
        if (!_balls.Contains(ball))
            _balls.Add(ball);
    }

    public void UnregisterBall(Ball ball)
    {
        if (_balls.Contains(ball))
            _balls.Remove(ball);

        if (_balls.Count == 0)
            GameManager.Instance.RemoveLife(1);
    }

    public List<Ball> GetBalls()
    {
        return _balls;
    }

    public void Clear()
    {
        foreach (Ball b in _balls)
        {
            if (b != null)
                Destroy(b.gameObject);
        }
        _balls.Clear();
    }

    public void SpawnExtraBall()
    {
        Ball newBall = Instantiate(_ballPrefab, Vector2.zero, Quaternion.identity);
        newBall.Launch();
        RegisterBall(newBall);
    }

    public void SpawnShotgunBalls(Vector2 paddlePosition)
    {
        float[] angles = { -30f, 0f, 30f };
        foreach (float angle in angles)
        {
            Ball newBall = Instantiate(_ballPrefab, paddlePosition, Quaternion.identity);
            newBall.Launch(angle);
            RegisterBall(newBall);
        }
    }

    public void MultiplyBalls()
    {
        List<Ball> currentBalls = new List<Ball>(_balls);
        foreach (Ball ball in currentBalls)
        {
            Ball clone = Instantiate(_ballPrefab, ball.transform.position, Quaternion.identity);
            clone.Launch(Random.Range(-45f, 45f));
            RegisterBall(clone);
        }
    }
}