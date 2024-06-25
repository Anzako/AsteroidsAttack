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
    [SerializeField] private Metaball boss2Metaball;
    private bool bossSpawned = false;
    private GameObject bossObject;
    public int actualBoss;

    // Testing
    public bool spawnAsteroids = true;

    // Sounds
    [SerializeField] private AudioClip bossSoundClip;
    [SerializeField] private AudioClip newWaveSoundClip;
    [SerializeField] private AudioClip ufoSpawnSoundClip;

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
        if (actualRound != 1)
        {
            SoundFXManager.Instance.PlaySoundFXClip(newWaveSoundClip, transform, 1f);
        }
        

        if (normalWavesAmount > actualRound - 1)
        {
            SetRoundVariables();
            SpawnAsteroids();
        } 
        else
        {
            OnRandomRounds();
        }

        playerHUD.SetWave(actualRound);
    }

    private void OnRandomRounds()
    {
        asteroidsSpawner.SpawnRandomAsteroids(actualRound);
        enemyLevel = 2;

        if (actualRound < 10)
        {
            timeToSpawnEnemy = 15;
            spawningEnemiesAmount = 2;
        }

        if (actualRound < 12)
        {
            timeToSpawnEnemy = 10;
            spawningEnemiesAmount = 2;
        }

        if (actualRound < 14)
        {
            timeToSpawnEnemy = 15;
            spawningEnemiesAmount = 3;
        }

        if (actualRound >= 14)
        {
            timeToSpawnEnemy = 15;
            spawningEnemiesAmount = 4;
        }
    }

    private void SetRoundVariables()
    {
        canEnemySpawn = waves[actualRound - 1].isEnemySpawning;
        spawningEnemiesAmount = waves[actualRound - 1].spawningEnemiesAmount;
        timeToSpawnEnemy = waves[actualRound - 1].timeToSpawnEnemy;
        enemyLevel = waves[actualRound - 1].enemyLevel;
    }
    
    private void OnBossRound()
    {
        actualBoss += 1;
        if (actualBoss == 1)
        {
            MetaBalls.Instance.AddMetaball(boss1Metaball);
        }
        else
        {
            MetaBalls.Instance.AddMetaball(boss2Metaball);
        }
        
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
        actualBoss = 0;
        spawningEnemiesAmount = 1;
        enemyLevel = 1;
        canEnemySpawn = false;

        ObjectPooler.Instance.ReturnAllObjectsToPool();

        MetaBalls.Instance.ResetMetaballsParameters();
        asteroidsSpawner.ResetAsteroidAmount();
        ScoreManager.Instance.ResetScore();

        if (bossObject != null)
        {
            Destroy(bossObject);
        }
    }

    public void EndRound()
    {
        if (gameManager.State != GameState.EndGame)
        {
            if (UpgradesManager.upgradesExhausted)
            {
                OnNewRound();
            } else
            {
                gameManager.ChangeState(GameState.UpgradeMenu);
            }
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
        SoundFXManager.Instance.PlaySoundFXClip(ufoSpawnSoundClip, transform, 1f);
    }

    private void SpawnBoss()
    {
        if (actualBoss == 1)
        {
            if (boss1Metaball.IsInsideMarchingBox())
            {
                SpawnPosition spawnPosition = Spawner.RandomSpawnPositionOnMetaball(boss1Metaball);
                bossObject = Instantiate(waves[actualRound - 1].bossGameObject, spawnPosition.position, spawnPosition.rotation);
                bossObject.GetComponent<EnemyHealth>().Killed += EndRound;
                bossSpawned = true;
                SoundFXManager.Instance.PlaySoundFXClip(bossSoundClip, transform, 1f);
            }
        } else
        {
            if (boss2Metaball.IsInsideMarchingBox())
            {
                SpawnPosition spawnPosition = Spawner.RandomSpawnPositionOnMetaball(boss2Metaball);
                bossObject = Instantiate(waves[actualRound - 1].bossGameObject, spawnPosition.position, spawnPosition.rotation);
                bossObject.GetComponent<EnemyHealth>().Killed += EndRound;
                bossSpawned = true;
                SoundFXManager.Instance.PlaySoundFXClip(bossSoundClip, transform, 1f);
            }
        }
        
    }

    private void SpawnAsteroids()
    {
        int asteroidLevel = waves[actualRound - 1].asteroidLevel;
        asteroidsSpawner.SpawnSmallAsteroids(waves[actualRound - 1].smallAsteroidAmount, asteroidLevel);
        asteroidsSpawner.SpawnMediumAsteroids(waves[actualRound - 1].mediumAsteroidAmount, asteroidLevel);
        asteroidsSpawner.SpawnBigAsteroids(waves[actualRound - 1].bigAsteroidAmount, asteroidLevel);

        if (waves[actualRound - 1].isBoss)
        {
            OnBossRound();
        }
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
