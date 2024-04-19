using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidsSpawner : Spawner
{
    [SerializeField] private PlayerController pController;

    public List<GameObject> asteroids;
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
            asteroids = new List<GameObject> ();
        }
    }

    #endregion

    private void OnEnable()
    {
        StartCoroutine(debugSth());
    }

    public IEnumerator debugSth()
    {
        Debug.Log(asteroids.Count);
        yield return new WaitForSeconds(1f);
        yield return debugSth();
    }

    public void SpawnAsteroids(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnAsteroidRandomOnMetaball();
        }
    }

    // Spawn new asteroid on random metaball
    private void SpawnAsteroidRandomOnMetaball()
    {
        asteroids.Add(SpawnGameObject(Random.Range(1, MetaBalls.instance.numberOfMetaballs), 
            asteroidSize[Random.Range(0, asteroidSize.Length)]));
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
        GameObject asteroid = instance.SpawnGameObject(size, position, rotation);
        asteroids.Add(asteroid);
        return asteroid;
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

    public void ReturnToPool(GameObject asteroid)
    {
        asteroids.Remove(asteroid);
        ObjectPooler.instance.ReturnObjectToPool(asteroid);
    }

    public void ResetGame()
    {
        for(int i = asteroids.Count - 1; i >= 0; i--)
        {
            ReturnToPool(asteroids[i]);
        }

        SpawnAsteroids(5);
    }
}
