using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LevelManager levelManager;

    public static event Action<GameState> OnStateChanged;

    public GameState State { get; private set; }

    private void Start()
    {
        ChangeState(GameState.Menu);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Menu:
                HandleMenu();
                break;
            case GameState.Game:
                HandleGame();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }

        OnStateChanged?.Invoke(newState);
        //Debug.Log($"New state: {newState}");
    }

    private void HandleMenu()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void HandleGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        levelManager.StartGame();
    }

    private void HandleGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}

[Serializable]
public enum GameState
{
    Menu = 0,
    Game = 1,
    GameOver = 2,
}