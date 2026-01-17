using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    GameOver,
    Win
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private BrickGenerator _brickGenerator;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Paddle _paddle;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _winUI;

    public int _score { get; private set; }
    public int _lives { get; private set; } = 3;
    public GameState State { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        State = GameState.Playing;
        _gameOverUI.SetActive(false);
        _winUI.SetActive(false);

        _brickGenerator.GenerateBricks();
        StartTurn();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_scoreText != null) _scoreText.text = _score.ToString("D3");
        if (_livesText != null) _livesText.text = _lives.ToString();
    }

    public void AddScore(int points)
    {
        if (State != GameState.Playing) return;

        _score += points;
        UpdateUI();

        if (_brickGenerator.AllBricksDestroyed())
            Win();
    }

    public void RemoveLife(int amount = 1)
    {
        if (State != GameState.Playing) return;

        _lives -= amount;
        UpdateUI();

        if (_lives <= 0)
            GameOver();
        else
            StartTurn();
    }

    private void StartTurn()
    {
        _paddle.ResetPosition();

        Ball newBall = Instantiate(_ballPrefab);
        newBall.AttachToPaddle(_paddle.transform);
        BallManager.Instance.RegisterBall(newBall);
    }

    private void GameOver()
    {
        State = GameState.GameOver;
        _gameOverUI.SetActive(true);
        FreezeAllBalls();
    }

    private void Win()
    {
        State = GameState.Win;
        _winUI.SetActive(true);
        FreezeAllBalls();
    }

    private void FreezeAllBalls()
    {
        foreach (Ball b in BallManager.Instance.GetBalls())
            Destroy(b.gameObject);
    }

    public void Retry()
    {
        _score = 0;
        _lives = 3;
        UpdateUI();

        foreach (Transform child in _brickGenerator.transform)
            Destroy(child.gameObject);

        BallManager.Instance.Clear();
        _brickGenerator.GenerateBricks();

        State = GameState.Playing;
        _gameOverUI.SetActive(false);
        _winUI.SetActive(false);

        StartTurn();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}