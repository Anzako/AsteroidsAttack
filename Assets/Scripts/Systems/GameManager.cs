using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private LevelManager levelManager;
    [SerializeField] private Camera menuCamera;
    [SerializeField] private PlayerController playerController;

    public static event Action<GameState> OnStateChanged;

    public GameState State { get; private set; }

    private void Start()
    {
        ChangeState(GameState.Menu);
        levelManager = LevelManager.Instance;
        OnStateChanged += GameManagerOnStateChanged;
    }

    private void OnDestroy()
    {
        OnStateChanged -= GameManagerOnStateChanged;
    }

    public void GameManagerOnStateChanged(GameState state)
    {
        if (state == GameState.StartGame) 
        {
            ChangeState(GameState.Game);
        }
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Menu:
                HandleMenu();
                break;
            case GameState.StartGame:
                HandleStartGame();
                break;
            case GameState.Game:
                HandleGame();
                break;
            case GameState.EndGame:
                HandleGameOver();
                break;
            case GameState.InGameMenu:
                HandleInGameMenu();
                break;
            case GameState.UpgradeMenu:
                HandleUpgradeMenu();
                break;
        }

        OnStateChanged?.Invoke(newState);
    }

    private void HandleMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        playerController.DisablePlayer();
        menuCamera.gameObject.SetActive(true);
    }

    private void HandleStartGame()
    {
        levelManager.StartGame();
        menuCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    private void HandleGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UnFreezeGame();
    }

    private void HandleGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        playerController.DisablePlayer();
        menuCamera.gameObject.SetActive(true);
    }

    private void HandleInGameMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        menuCamera.gameObject.SetActive(false);
        FreezeGame();
    }

    private void HandleUpgradeMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        FreezeGame();
    }

    public void FreezeGame()
    {
        Time.timeScale = 0;
        playerController.Freeze(true);
    }

    public void UnFreezeGame()
    {
        Time.timeScale = 1;
        playerController.Freeze(false);
    }

    public static PlayerController GetPlayerController()
    {
        return Instance.playerController;
    }
}



[Serializable]
public enum GameState
{
    Menu = 0,
    StartGame = 1,
    Game = 2,
    InGameMenu = 3,
    UpgradeMenu = 4,
    EndGame = 5,
}