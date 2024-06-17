using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private GameManager gameManager;
    private PlayerSpawner playerSpawner;
    private AsteroidsSpawner asteroidsSpawner;
    private EnemySpawner enemySpawner;

    private PlayerController playerController;
    private UIController playerHUD;

    private bool gameStarted = false;
    private int actualRound = 1;
    private float elapsedTime = 0;
    private float elapsedRoundTime = 0;
    public float timeToSpawnEnemy;

    private int enemyLevel = 1;
    private int spawningEnemiesAmount = 1;
    private int asteroidsAmount;
    private int asteroidsLevel;


    // Testing
    public bool spawnAsteroids = true;

    private void Start()
    {
        playerSpawner = PlayerSpawner.Instance;
        asteroidsSpawner = AsteroidsSpawner.Instance;
        gameManager = GameManager.Instance;
        enemySpawner = EnemySpawner.Instance;

        playerController = GameManager.GetPlayerController();
        playerHUD = playerController.GetComponent<UIController>();
    }

    private void Update()
    {
        if (!gameStarted) return;

        elapsedTime += Time.deltaTime;
        elapsedRoundTime += Time.deltaTime;

        if (elapsedRoundTime >= timeToSpawnEnemy)
        {
            SpawnEnemy();
            elapsedRoundTime = 0;
        }
    }

    #region Game Logic
    public void StartGame()
    {
        CleanScene();
        
        gameStarted = true;
        actualRound = 1;
        elapsedTime = 0;

        playerSpawner.SpawnPlayer(); 
        StartRound();
    }

    private void StartRound()
    {
        elapsedRoundTime = 0;

        if (spawnAsteroids)
        {
            //asteroidsSpawner.SpawnAsteroids(asteroidsAmount);
            asteroidsSpawner.SpawnAsteroids(1);
        }

        playerHUD.SetWave(actualRound);
        OnRoundStart();
    }

    private void OnRoundStart()
    {
        asteroidsAmount = (actualRound + 1) * 2;
        if (actualRound == 3)
        {
            spawningEnemiesAmount = 2;
        }

        if (actualRound == 5)
        {
            enemyLevel = 2;
            spawningEnemiesAmount = 1;
        }
    }

    public void RestartGame()
    {
        gameManager.ChangeState(GameState.StartGame);
    }

    public void CleanScene()
    {
        spawningEnemiesAmount = 1;
        enemyLevel = 1;

        MetaBalls.Instance.ResetMetaballsParameters();
        asteroidsSpawner.DestroyAllAsteroids();
        enemySpawner.DestroyAllEnemies();
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.playerProjectile);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.rocket);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.healUpItem);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.laserItem);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.rocketItem);
        ScoreManager.Instance.ResetScore();
    }

    public void EndRound()
    {
        actualRound++;

        if (gameManager.State != GameState.EndGame)
        {
            gameManager.ChangeState(GameState.UpgradeMenu);
        }
        
        StartRound();
    }

    public void EndGame()
    {
        gameManager.ChangeState(GameState.EndGame);
        gameStarted = false;
    }
    #endregion

    private void SpawnEnemy()
    {
        for (int i = 0; i < spawningEnemiesAmount; i++)
        {
            enemySpawner.SpawnEnemy(enemyLevel);
        }
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public Vector3 GetPlayerPosition()
    {
        return playerController.transform.position;
    }
}
