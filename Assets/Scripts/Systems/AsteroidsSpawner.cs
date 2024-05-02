using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;


public class AsteroidsSpawner : Singleton<AsteroidsSpawner>
{
    [SerializeField] private PlayerController pController;
    [SerializeField] private Camera pCamera;
    private Spawner spawner;

    private List<GameObject> asteroids;
    public string[] asteroidSize;

    private void Start()
    {
        asteroids = new List<GameObject>();
        spawner = Spawner.Instance;
        AsteroidController.onDestroy += CheckIfAnyAsteroidsLeft;
    }

    private void OnDestroy()
    {
        AsteroidController.onDestroy -= CheckIfAnyAsteroidsLeft;
    }

    public IEnumerator debugSth()
    {
        Debug.Log(asteroids.Count);
        yield return new WaitForSeconds(1f);
        yield return debugSth();
    }

    public void CheckIfAnyAsteroidsLeft()
    {
        if (asteroids.Count == 0) { LevelManager.Instance.EndRound(); }
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
        string randomAsteroidSize = asteroidSize[Random.Range(0, asteroidSize.Length)];
        
        asteroids.Add(Spawner.Instance.SpawnAwayFromPlayerView(randomAsteroidSize, -pController.transform.up));
        //asteroids.Add(SpawnAwayFromPlayerView(randomAsteroidSize, pCamera.transform.forward));
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
        GameObject asteroid = spawner.SpawnPoolObjectOnPosition(size, position, rotation);
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

        SpawnRandomAsteroid();
    }

    public void ReturnAsteroidsToPool()
    {
        for (int i = asteroids.Count - 1; i >= 0; i--)
        {
            ReturnToPool(asteroids[i]);
        }
    }

    public void ReturnToPool(GameObject asteroid)
    {
        asteroids.Remove(asteroid);
        ObjectPooler.instance.ReturnObjectToPool(asteroid);
    }
}
