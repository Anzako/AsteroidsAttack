using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AsteroidsSpawner : Spawner
{
    public int amount;
    [SerializeField] private PlayerController pController;

    private bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (!spawned)
        {
            SpawnAsteroid();
            SpawnAsteroid();
            SpawnAsteroid();
            SpawnAsteroid();
            SpawnAsteroid();
            spawned = true;
        }
    }

    private void SpawnAsteroid()
    {
        GameObject asteroid = SpawnGameObject(0);
        asteroid.GetComponentInChildren<AsteroidsHealth>().Killed += pController.AddScore;
    }

    
}
