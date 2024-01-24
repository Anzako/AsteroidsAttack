using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private MetaBalls metaballs;
    [SerializeField] private GameObject enemyPrefab;

    bool enemiesSpawned = false;

    // Update is called once per frame
    void Update()
    {
        if (!enemiesSpawned)
        {
            SpawnEnemy(0);
            SpawnEnemy(1);
            SpawnEnemy(1);
            SpawnEnemy(1);
            SpawnEnemy(1);
            SpawnEnemy(1);
            SpawnEnemy(1);
            SpawnEnemy(1);
            enemiesSpawned = true;
        }
    }

    private void SpawnEnemy(int metaballID)
    {
        Vector3 pos = metaballs.Position(metaballID);
        float radius = metaballs.Radius(metaballID);

        Vector3 directionFromCenter = CalculateRandomVector3();

        while(!CheckCanEnemySpawn(directionFromCenter, metaballID)) 
        {
            directionFromCenter = CalculateRandomVector3();
        }

        Vector3 spawnPosition = pos + directionFromCenter.normalized * (radius + 1);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - pos);

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, rotation, gameObject.transform);
        HealthController enemy = newEnemy.GetComponentInChildren<HealthController>();
        if (enemy != null)
        {
            enemy.Killed += OnEnemyKill;
        }
    }

    private Vector3 CalculateRandomVector3()
    {
        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);
        float randZ = Random.Range(-1f, 1f);

        return new Vector3(randX, randY, randZ);
    }

    public void OnEnemyKill()
    {
        SpawnEnemy(0);
    }

    private bool CheckCanEnemySpawn(Vector3 direction, int metaballID)
    {
        for (int i = 0; i < metaballs.numberOfMetaballs; i++)
        {
            if (i == metaballID)
            {
                continue;
            }

            Vector3 metaballsVector = metaballs.Position(i) - metaballs.Position(metaballID);
            float sumOfRadius = metaballs.Radius(i) + metaballs.Radius(metaballID);

            if (metaballsVector.magnitude < sumOfRadius + 8)
            {
                float angle = Vector3.Angle(direction, metaballsVector);
                if (angle <= 50)
                {
                    return false;
                }
            }
            
        }
        return true;
    }

}
