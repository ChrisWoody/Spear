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
    public Crosshair crosshair;

    public Transform easyDifficulty;
    public Transform normalDifficulty;
    public Text gameOverDifficultyLabel;
    public Text gameOverHighScoreDifficultyLabel;
    public Transform gameOverEasyDifficulty;
    public Transform gameOverNormalDifficulty;

    private void Start()
    {
        mainMenuStartGameButton.enabled = true;
        mainMenuCanvas.enabled = true;
        gameOverStartGameButton.enabled = false;
        gameOverCanvas.enabled = false;
        gameCanvas.enabled = false;
        crosshair.Hide();

        easyDifficulty.GetComponent<Image>().color = Color.grey;
        normalDifficulty.GetComponent<Image>().color = Color.white;

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
        crosshair.Show();
        
        GameController.GameStarted();
    }

    private void GameOver()
    {
        mainMenuStartGameButton.enabled = false;
        mainMenuCanvas.enabled = false;
        gameOverStartGameButton.enabled = true;
        gameOverCanvas.enabled = true;
        gameCanvas.enabled = false;
        crosshair.Hide();

        gameOverEasyDifficulty.GetComponent<Image>().color = GameController.Difficulty == Difficulty.Easy ? Color.white : Color.grey;
        gameOverNormalDifficulty.GetComponent<Image>().color = GameController.Difficulty == Difficulty.Normal ? Color.white : Color.grey;

        gameOverDifficultyLabel.text = $"Difficulty: {GameController.Difficulty}";
        gameOverScore.text = $"Score: {GameController.Score}";
        gameOverHighScore.text = $"High Score: {GameController.HighScore}";
        if (GameController.Score == GameController.HighScore) // so if it's a high score, also set the difficulty it was attained with
            gameOverHighScoreDifficultyLabel.text = $"Difficulty: {GameController.Difficulty}";
    }

    public void SetDifficultyEasy()
    {
        easyDifficulty.GetComponent<Image>().color = Color.white;
        normalDifficulty.GetComponent<Image>().color = Color.grey;
        gameOverEasyDifficulty.GetComponent<Image>().color = Color.white;
        gameOverNormalDifficulty.GetComponent<Image>().color = Color.grey;
        GameController.SetDifficulty(Difficulty.Easy);
    }

    public void SetDifficultyNormal()
    {
        easyDifficulty.GetComponent<Image>().color = Color.grey;
        normalDifficulty.GetComponent<Image>().color = Color.white;
        gameOverEasyDifficulty.GetComponent<Image>().color = Color.grey;
        gameOverNormalDifficulty.GetComponent<Image>().color = Color.white;
        GameController.SetDifficulty(Difficulty.Normal);
    }

    public void SetEnemyDeathExtreme(bool value)
    {
        GameController.SetEnemyDeathEffect(value);
    }
}
