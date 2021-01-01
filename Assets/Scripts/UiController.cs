using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Canvas mainMenuCanvas;
    public Button mainMenuStartGameButton;
    public Canvas gameOverCanvas;
    public Button gameOverStartGameButton;
    public Canvas gameCanvas;
    public Text gameOverScore;
    public Text gameOverHighScore;
    public Text gameScore;
    public Text gameHighScore;

    private void Start()
    {
        mainMenuStartGameButton.enabled = true;
        mainMenuCanvas.enabled = true;
        gameOverStartGameButton.enabled = false;
        gameOverCanvas.enabled = false;
        gameCanvas.enabled = false;

        GameController.OnStartGame += GameControllerOnOnEnemyKilled;
        GameController.OnGameOver += GameOver;
        GameController.OnEnemyKilled += GameControllerOnOnEnemyKilled;
    }

    private void GameControllerOnOnEnemyKilled()
    {
        gameScore.text = "Score: " + GameController.Score;
        gameHighScore.text = "High Score: " + GameController.HighScore;
    }

    public void StartGame()
    {
        mainMenuStartGameButton.enabled = false;
        mainMenuCanvas.enabled = false;
        gameOverStartGameButton.enabled = false;
        gameOverCanvas.enabled = false;
        gameCanvas.enabled = true;
        
        GameController.GameStarted();
    }

    private void GameOver()
    {
        mainMenuStartGameButton.enabled = false;
        mainMenuCanvas.enabled = false;
        gameOverStartGameButton.enabled = true;
        gameOverCanvas.enabled = true;
        gameCanvas.enabled = false;

        gameOverScore.text = "Score: " + GameController.Score;
        gameOverHighScore.text = "High Score: " + GameController.HighScore;
    }
}
