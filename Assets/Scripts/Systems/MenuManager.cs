using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : Singleton<MenuManager>
{
    // Main menu
    [SerializeField] private GameObject mainMenu;

    // In game menu
    [SerializeField] private GameObject inGameMenu;

    // End game menu
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private TMP_Text scoreText;

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
        mainMenu.SetActive(state == GameState.Menu);

        // In game menu
        inGameMenu.SetActive(state == GameState.InGameMenu);

        // End game menu
        endGameUI.SetActive(state == GameState.EndGame);
        scoreText.gameObject.SetActive(state == GameState.EndGame);
        if (state == GameState.EndGame) 
        {
            SetScore();
        }
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

    public void SetScore()
    {
        scoreText.text = "Your Score: " + ScoreManager.Instance.GetScore();
    }

}
