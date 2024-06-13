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
            poolTags.smallAsteroid,
            poolTags.mediumAsteroid,
            poolTags.bigAsteroid
        };
    }

    public void OnAsteroidDestroy()
    {
        SetAsteroidsAmount(asteroidsAmount - 1);
        if (asteroidsAmount == 0) { LevelManager.Instance.EndRound(); }
    }

    public void SpawnAsteroids(int amount)
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

    // Spawn asteroid close to destroyed one
    public void SpawnAsteroidOnDestroy(poolTags size, Transform transform)
    {
        float distanceOfSpawn = 0.5f;
        float randomAngle = Random.Range(0f, 360f);
        Transform newTransform = transform;
        newTransform.RotateAround(newTransform.position, newTransform.up, randomAngle);
        newTransform.position += newTransform.forward * distanceOfSpawn;

        SpawnAsteroid(size, newTransform.position, newTransform.rotation);
    }


    public GameObject SpawnAsteroid(poolTags size, Vector3 position, Quaternion rotation)
    {
        GameObject asteroid = Spawner.SpawnPoolObjectOnPosition(size, position, rotation);
        SetAsteroidsAmount(asteroidsAmount + 1);

        return asteroid;
    }

    private void SetAsteroidsAmount(int amount)
    {
        asteroidsAmount = amount;
        pController.GetComponent<UIController>().SetAsteroidsAmountText(amount);
    }

    public void DestroyAllAsteroids()
    {
        pooler.ReturnObjectsToPool(poolTags.smallAsteroid);
        pooler.ReturnObjectsToPool(poolTags.mediumAsteroid);
        pooler.ReturnObjectsToPool(poolTags.bigAsteroid);
        pooler.ReturnObjectsToPool(poolTags.healUpAsteroid);
        SetAsteroidsAmount(0);
    }

    public void SpawnAsteroidInTime(float time)
    {
        StartCoroutine(WaitAndSpawn(time));
    }

    public IEnumerator WaitAndSpawn(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnRandomAsteroid();
    }
}
