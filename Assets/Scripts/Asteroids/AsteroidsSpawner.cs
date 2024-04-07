using System;
using System.Collections;
using UnityEngine;
using static AsteroidController;
using Random = UnityEngine.Random;

public class AsteroidsSpawner : Spawner
{
    [SerializeField] private PlayerController pController;
    
    private bool spawned = false;
    public static string objectTag = "asteroid";

    #region Singleton

    public static AsteroidsSpawner instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    void Update()
    {
        // Spawn asteroids at game start
        if (!spawned)
        {
            SpawnAsteroids(4);
            spawned = true;
        }
    }

    private void SpawnAsteroids(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnAsteroidRandomOnMetaball();
        }
    }

    // Spawn new asteroid on random metaball
    private void SpawnAsteroidRandomOnMetaball()
    {
        // spawn asteroid on metaballs without start metaball
        GameObject asteroid = SpawnGameObject(Random.Range(1, MetaBalls.instance.numberOfMetaballs), objectTag);

        AsteroidController asteroidController = asteroid.GetComponent<AsteroidController>();

        Array values = Enum.GetValues(typeof(AsteroidSize));
        System.Random random = new System.Random();
        AsteroidSize randomSize = (AsteroidSize)values.GetValue(random.Next(values.Length));

        asteroidController.Size = randomSize;
    }

    // Spawn asteroid close to destroyed one
    public static void SpawnAsteroidOnDestroy(AsteroidSize size, Transform transform)
    {
        float distanceOfSpawn = 0.5f;
        float randomAngle = Random.Range(0f, 360f);
        Transform newTransform = transform;
        newTransform.RotateAround(newTransform.position, newTransform.up, randomAngle);
        newTransform.position += newTransform.forward * distanceOfSpawn;

        GameObject spawnedAsteroid = SpawnAsteroid(newTransform.position, newTransform.rotation);
        AsteroidController asteroidController = spawnedAsteroid.GetComponent<AsteroidController>();

        // create setter and getter for _size
        asteroidController.Size = size;
    }

    public static GameObject SpawnAsteroid(Vector3 position, Quaternion rotation)
    {
        return instance.SpawnGameObject(objectTag, position, rotation);
    }

    public void SpawnAsteroidInTime(float time)
    {
        StartCoroutine(WaitAndSpawn(time));
    }

    public IEnumerator WaitAndSpawn(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnAsteroidRandomOnMetaball();
    }

}
