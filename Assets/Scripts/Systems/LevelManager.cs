using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private GameManager gameManager;
    private PlayerSpawner pSpawner;
    private AsteroidsSpawner aSpawner;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIController HUDController;

    private bool gameStarted = false;
    private int actualRound = 0;
    private float elapsedTime = 0;

    public int[] asteroidsInRound;

    private void Start()
    {
        pSpawner = PlayerSpawner.Instance;
        aSpawner = AsteroidsSpawner.Instance;
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (!gameStarted) return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 30f)
        {
            SpawnEnemy();
        }
    }

    #region Game Logic
    public void StartGame()
    {
        CleanScene();
        actualRound = 0;
        elapsedTime = 0;
        gameStarted = true;

        pSpawner.SpawnPlayer(); 
        StartRound(actualRound);
    }

    private void StartRound(int round)
    {
        aSpawner.SpawnAsteroids(asteroidsInRound[round]);
        HUDController.SetWave(round + 1);
    }

    public void RestartGame()
    {
        CleanScene();
        gameManager.ChangeState(GameState.StartGame);
    }

    public void CleanScene()
    {
        aSpawner.DestroyAllAsteroids();
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.playerProjectile);
        ScoreManager.Instance.ResetScore();
    }

    public void EndRound()
    {
        actualRound++;
        if (actualRound >= asteroidsInRound.Length)
        {
            EndGame();
            return;
        } 

        // Do as IEnumerator to wait for next round
        StartRound(actualRound);
    }

    public void EndGame()
    {
        gameManager.ChangeState(GameState.EndGame);
        gameStarted = false;
    }
    #endregion

    private void SpawnEnemy()
    {

    }
}
