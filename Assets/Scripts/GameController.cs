using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0f;
    }

    public static void GameStarted()
    {
        IsGameRunning = true;
        Time.timeScale = 1f;
        Score = 0;
        OnStartGame?.Invoke();
    }

    public static void GameOver()
    {
        IsGameRunning = false;
        Time.timeScale = 0f;
        OnGameOver?.Invoke();
    }

    public static bool IsGameRunning { get; private set; }

    public static event Action OnStartGame;
    public static event Action OnGameOver;
    public static event Action OnEnemyKilled;

    public static int Score { get; private set; }
    public static int HighScore { get; private set; }
    
    public static void EnemyKilled()
    {
        Score++;
        if (Score > HighScore)
            HighScore = Score;
        OnEnemyKilled?.Invoke();
    }

    public static Difficulty Difficulty { get; private set; } = Difficulty.Normal;
    
    public static void SetDifficulty(Difficulty difficulty)
    {
        Difficulty = difficulty;
    }
}

public enum Difficulty
{
    Normal,
    Easy
}
