using UnityEngine;

public class UiController : MonoBehaviour
{
    public Canvas mainMenuCanvas;
    public Canvas gameOverCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        mainMenuCanvas.enabled = true;
        gameOverCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        mainMenuCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        GameController.GameStarted();
    }

    public void GameOver()
    {
        gameOverCanvas.enabled = true;
    }
}
