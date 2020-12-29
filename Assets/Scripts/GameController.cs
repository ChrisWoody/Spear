using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static bool _isGameRunning;
    
    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GameStarted()
    {
        _isGameRunning = true;
        Time.timeScale = 1f;
    }

    public static bool IsGameRunning() => _isGameRunning;
}
