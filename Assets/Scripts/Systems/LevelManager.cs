using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerSpawner pSpawner;
    [SerializeField] private AsteroidsSpawner aSpawner;
    [SerializeField] private PlayerHealth pHealth;
    [SerializeField] private Button restartButton;
    public int amount;

    private void Start()
    {
        pHealth.Killed += GameOver;
        restartButton.onClick.AddListener(RestartGame);
    }

    private void Awake()
    {
        GameManager.OnStateChanged += GameManagerOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= GameManagerOnStateChanged;
    }

    public void GameManagerOnStateChanged(GameState state)
    {
        restartButton.gameObject.SetActive(state == GameState.GameOver);
    }

    public void StartGame()
    {
        pSpawner.SpawnPlayer();
        StartRound(0);
    }

    public void GameOver()
    {
        ScoreManager.instance.ResetScore();
        GameManager.Instance.ChangeState(GameState.GameOver);
    }

    public void RestartGame()
    {
        aSpawner.ReturnAsteroidsToPool();
        GameManager.Instance.ChangeState(GameState.Game);
    }

    private void StartRound(int round)
    {
        aSpawner.SpawnAsteroids(amount);
    }
}
