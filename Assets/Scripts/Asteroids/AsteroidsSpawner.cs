using System.Collections;
using UnityEngine;

public class AsteroidsSpawner : Spawner
{
    [SerializeField] private PlayerController pController;
    
    private bool spawned = false;
    public string objectTag = "asteroid";

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
        if (!spawned)
        {
            SpawnAsteroids(6);
            spawned = true;
        }
    }

    private void SpawnAsteroids(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnAsteroid();
        }
    }

    private void SpawnAsteroid()
    {
        GameObject obj = SpawnGameObject(Random.Range(0, MetaBalls.instance.numberOfMetaballs), objectTag);
    }

    public void SpawnAsteroidInTime(float time)
    {
        StartCoroutine(WaitAndSpawn(time));
    }

    public IEnumerator WaitAndSpawn(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnAsteroid();
    }

}
