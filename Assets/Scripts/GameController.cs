using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static UiController _uiController;
    
    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 0f;
        _uiController = FindObjectOfType<UiController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GameStarted()
    {
        IsGameRunning = true;
        Time.timeScale = 1f;
        OnStartGame?.Invoke();
    }

    public static void GameOver()
    {
        IsGameRunning = false;
        Time.timeScale = 0f;

        if (_uiController != null)
            _uiController.GameOver();
    }

    public static bool IsGameRunning { get; private set; }

    public static event Action OnStartGame;
}
