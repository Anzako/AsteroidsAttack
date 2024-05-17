using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : Singleton<MenuManager>
{
    // Main menu
    [SerializeField] private GameObject startButton;
    [SerializeField] private TMP_Text mainMenuText;

    // In game menu
    [SerializeField] private TMP_Text pauseText;
    [SerializeField] private Button resumeGameButton;
    [SerializeField] private Button mainMenuButton; 
    
    // End game menu
    [SerializeField] private Button restartButton;
    

    private void Awake()
    {
        GameManager.OnStateChanged += GameManagerOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState state)
    {
        // Menu
        startButton.SetActive(state == GameState.Menu);
        mainMenuText.gameObject.SetActive(state == GameState.Menu);

        // In game menu
        resumeGameButton.gameObject.SetActive(state == GameState.InGameMenu);
        mainMenuButton.gameObject.SetActive(state == GameState.InGameMenu);
        pauseText.gameObject.SetActive(state == GameState.InGameMenu);

        // End game menu
        restartButton.gameObject.SetActive(state == GameState.EndGame);
    }

    public void StartPressed()
    {
        GameManager.Instance.ChangeState(GameState.StartGame);
    }

    public void RestartButtonPressed()
    {
        LevelManager.Instance.RestartGame();
    }

    public void ResumeGameButtonPressed()
    {
        GameManager.Instance.ChangeState(GameState.Game);
    }

    public void MainMenuButtonPressed()
    {
        GameManager.Instance.ChangeState(GameState.Menu);
    }

}
