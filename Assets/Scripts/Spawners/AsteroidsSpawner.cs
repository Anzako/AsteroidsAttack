using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawner : Singleton<AsteroidsSpawner>
{
    private PlayerController pController;
    private ObjectPooler pooler;

    private int asteroidsAmount = 0;
    private List<poolTags> asteroidsTags;

    private void Start()
    {
        pooler = ObjectPooler.Instance;
        pController = GameManager.GetPlayerController();

        asteroidsTags = new List<poolTags>
        {
            poolTags.smallAsteroid_lvl2,
            poolTags.mediumAsteroid_lvl2,
            poolTags.bigAsteroid_lvl2
        };
    }

    public void OnAsteroidDestroy()
    {
        SetAsteroidsAmount(asteroidsAmount - 1);
        if (asteroidsAmount == 0) { LevelManager.Instance.EndRound(); }
    }

    public void SpawnSmallAsteroids(int amount, int level)
    {
        for (int i = 0; i < amount; i++)
        {
            if (level == 1)
            {
                SpawnAsteroid(poolTags.smallAsteroid_lvl1);
            } else if (level == 2)
            {
                SpawnAsteroid(poolTags.smallAsteroid_lvl2);
            }
            
        }
    }

    public void SpawnMediumAsteroids(int amount, int level)
    {
        for (int i = 0; i < amount; i++)
        {
            if (level == 1)
            {
                SpawnAsteroid(poolTags.mediumAsteroid_lvl1);
            }
            else if (level == 2)
            {
                SpawnAsteroid(poolTags.mediumAsteroid_lvl2);
            }
        }
    }

    public void SpawnBigAsteroids(int amount, int level)
    {
        for (int i = 0; i < amount; i++)
        {
            if (level == 1)
            {
                SpawnAsteroid(poolTags.bigAsteroid_lvl1);
            }
            else if (level == 2)
            {
                SpawnAsteroid(poolTags.bigAsteroid_lvl2);
            }
        }
    }

    #region Random Asteroid Spawning
    public void SpawnRandomAsteroids(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnRandomAsteroid();
        }
    }

    // Spawn new asteroid on random metaball
    private void SpawnRandomAsteroid()
    {
        poolTags randomAsteroidSize = asteroidsTags[Random.Range(0, asteroidsTags.Count)];
        Spawner.SpawnAwayFromPlayerView(randomAsteroidSize);

        SetAsteroidsAmount(asteroidsAmount + 1);
    }
    #endregion

    // Spawn asteroid close to destroyed one
    public void SpawnAsteroidOnDestroy(poolTags asteroidTag, Transform transform)
    {
        float distanceOfSpawn = 0.5f;
        float randomAngle = Random.Range(0f, 360f);
        Transform newTransform = transform;
        newTransform.RotateAround(newTransform.position, newTransform.up, randomAngle);
        newTransform.position += newTransform.forward * distanceOfSpawn;

        SpawnAsteroid(asteroidTag, newTransform.position, newTransform.rotation);
    }

    public void SpawnAsteroid(poolTags asteroidTag)
    {
        Spawner.SpawnAwayFromPlayerView(asteroidTag);

        SetAsteroidsAmount(asteroidsAmount + 1);
    }

    public void SpawnAsteroid(poolTags asteroidTag, Vector3 position, Quaternion rotation)
    {
        Spawner.SpawnPoolObjectOnPosition(asteroidTag, position, rotation);
        SetAsteroidsAmount(asteroidsAmount + 1);
    }

    private void SetAsteroidsAmount(int amount)
    {
        asteroidsAmount = amount;

        string taskText = "Destroy asteroids: " + amount;
        pController.GetComponent<UIController>().SetTaskText(taskText);
    }

    public void ResetAsteroidAmount()
    {
        asteroidsAmount = 0;
    }

}
