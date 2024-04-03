using System.Collections;
using UnityEngine;
using static AsteroidController;

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
            SpawnAsteroids(1);
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

    private void SpawnAsteroidRandomOnMetaball()
    {
        SpawnGameObject(Random.Range(0, MetaBalls.instance.numberOfMetaballs), objectTag);
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
        asteroidController._size = size;
        asteroidController.UpdateAsteroidParameters();
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
