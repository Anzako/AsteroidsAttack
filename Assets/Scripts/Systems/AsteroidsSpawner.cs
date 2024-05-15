using System.Collections;
using UnityEngine;

public class AsteroidsSpawner : Singleton<AsteroidsSpawner>
{
    [SerializeField] private PlayerController pController;
    [SerializeField] private Camera pCamera;
    private Spawner spawner;

    private int asteroidsAmount = 0;
    public string[] asteroidSize;

    private void Start()
    {
        spawner = Spawner.Instance;
        AsteroidController.onDestroy += OnAsteroidDestroy;
    }

    private void OnDestroy()
    {
        AsteroidController.onDestroy -= OnAsteroidDestroy;
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
        string randomAsteroidSize = asteroidSize[Random.Range(0, asteroidSize.Length)];
        Spawner.Instance.SpawnAwayFromPlayerView(randomAsteroidSize, -pController.transform.up);

        SetAsteroidsAmount(asteroidsAmount + 1);
        //Spawner.Instance.SpawnAwayFromPlayerView(randomAsteroidSize, pCamera.transform.forward));
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
        ObjectPooler.Instance.ReturnObjectsToPool("smallAsteroid");
        ObjectPooler.Instance.ReturnObjectsToPool("mediumAsteroid");
        ObjectPooler.Instance.ReturnObjectsToPool("bigAsteroid");
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
