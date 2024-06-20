using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private GameManager gameManager;
    private PlayerSpawner playerSpawner;
    private AsteroidsSpawner asteroidsSpawner;
    private EnemySpawner enemySpawner;
    private PlayerController playerController;
    private UIController playerHUD;

    // Level variables
    private bool gameStarted = false;
    private int actualRound = 1;
    private float elapsedTime = 0;
    private float elapsedRoundTime = 0;

    // Wave variables
    [SerializeField] private List<Wave> waves;
    private int normalWavesAmount;

    public bool canEnemySpawn = true;
    public int enemyLevel = 1;
    public int spawningEnemiesAmount = 1;
    public float timeToSpawnEnemy;

    // Boss
    [SerializeField] private Metaball boss1Metaball;
    private bool bossSpawned = false;

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

        normalWavesAmount = waves.Count;
    }

    private void Update()
    {
        if (!gameStarted) return;

        elapsedTime += Time.deltaTime;

        if (isBossRound() && !bossSpawned)
        {
            SpawnBoss();
        }

        // Check if enemy spawn in round
        if (!canEnemySpawn) return;
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

        if (normalWavesAmount > actualRound - 1)
        {
            SetRoundVariables();
            SpawnAsteroids();
        } 
        else
        {
            SpawnRandomAsteroids();
        }

        playerHUD.SetWave(actualRound);
    }

    private void SetRoundVariables()
    {
        canEnemySpawn = waves[actualRound - 1].isEnemySpawning;
        spawningEnemiesAmount = waves[actualRound - 1].spawningEnemiesAmount;
        timeToSpawnEnemy = waves[actualRound - 1].timeToSpawnEnemy;
    }
    
    private void OnBossRound()
    {
        MetaBalls.Instance.AddMetaball(boss1Metaball);
        MetaBalls.Instance.OnBossStart();
        bossSpawned = false;

        playerHUD.SetTaskText("Kill the boss");
    }

    public void RestartGame()
    {
        gameManager.ChangeState(GameState.StartGame);
    }

    public void CleanScene()
    {
        spawningEnemiesAmount = 1;
        enemyLevel = 1;
        canEnemySpawn = false;

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
        if (gameManager.State != GameState.EndGame)
        {
            gameManager.ChangeState(GameState.UpgradeMenu);
        }

        if (isBossRound())
        {
            MetaBalls.Instance.OnBossEnd();
        }
    }

    public void OnNewRound()
    {
        actualRound++;
        StartRound();
    }

    public void EndGame()
    {
        gameManager.ChangeState(GameState.EndGame);
        gameStarted = false;
    }
    #endregion

    #region Spawning
    private void SpawnEnemy()
    {
        for (int i = 0; i < spawningEnemiesAmount; i++)
        {
            enemySpawner.SpawnEnemy(enemyLevel);
        }
    }

    private void SpawnBoss()
    {
        if (boss1Metaball.IsInsideMarchingBox())
        {
            SpawnPosition spawnPosition = Spawner.RandomSpawnPositionOnMetaball(boss1Metaball);
            GameObject boss = Instantiate(waves[actualRound - 1].bossGameObject, spawnPosition.position, spawnPosition.rotation);
            boss.GetComponent<EnemyHealth>().Killed += EndRound;
            bossSpawned = true;
        }
    }

    private void SpawnAsteroids()
    {
        asteroidsSpawner.SpawnSmallAsteroids(waves[actualRound - 1].smallAsteroidAmount);
        asteroidsSpawner.SpawnMediumAsteroids(waves[actualRound - 1].mediumAsteroidAmount);
        asteroidsSpawner.SpawnBigAsteroids(waves[actualRound - 1].bigAsteroidAmount);

        if (waves[actualRound - 1].isBoss)
        {
            OnBossRound();
        }
    }

    private void SpawnRandomAsteroids()
    {
        asteroidsSpawner.SpawnRandomAsteroids(6);
    }
    #endregion

    #region Getters

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public Vector3 GetPlayerPosition()
    {
        return playerController.transform.position;
    }

    public bool isBossRound()
    {
        if (waves.Count > actualRound - 1)
        {
            return waves[actualRound - 1].isBoss;
        }
        return false;
    }
    #endregion
}
