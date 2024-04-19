using System.Collections;
using UnityEngine;


public class AsteroidsSpawner : Spawner
{
    [SerializeField] private PlayerController pController;
    
    private bool spawned = false;
    public string[] asteroidSize;

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
        SpawnGameObject(Random.Range(1, MetaBalls.instance.numberOfMetaballs), 
            asteroidSize[Random.Range(0, asteroidSize.Length)]);
    }

    // Spawn asteroid close to destroyed one
    public void SpawnAsteroidOnDestroy(string size, Transform transform)
    {
        float distanceOfSpawn = 0.5f;
        float randomAngle = Random.Range(0f, 360f);
        Transform newTransform = transform;
        newTransform.RotateAround(newTransform.position, newTransform.up, randomAngle);
        newTransform.position += newTransform.forward * distanceOfSpawn;

        SpawnAsteroid(size, newTransform.position, newTransform.rotation);
    }


    public GameObject SpawnAsteroid(string size, Vector3 position, Quaternion rotation)
    {
        return instance.SpawnGameObject(size, position, rotation);
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
