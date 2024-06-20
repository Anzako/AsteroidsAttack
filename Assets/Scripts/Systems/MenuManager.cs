using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : Singleton<MenuManager>
{
    // Main menu
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;

    // In game menu
    [SerializeField] private GameObject inGameMenu;

    // End game menu
    [SerializeField] private GameObject endGameUI;

    // Upgrade menu
    [SerializeField] private GameObject upgradeMenu;

    protected override void Awake()
    {
        base.Awake();
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
        if (state == GameState.Menu)
        {
            settings.SetActive(false);
            credits.SetActive(false);
        }

        // In game menu
        inGameMenu.SetActive(state == GameState.InGameMenu);

        // End game menu
        endGameUI.SetActive(state == GameState.EndGame);
        endGameUI.transform.Find("SubmitButton").gameObject.SetActive(true);

        // Upgrade menu
        upgradeMenu.gameObject.SetActive(state == GameState.UpgradeMenu);
    }

    public void StartPressed()
    {
        GameManager.Instance.ChangeState(GameState.StartGame);
    }

    public void SettingsButtonPressed()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void CreditsButtonPressed()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
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

    public void QuitButtonPressed()
    {
        Application.Quit();
    }

}
