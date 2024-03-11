using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private MetaBalls metaballs;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private PlayerController playerController;

    private bool enemiesSpawned = false;
    public float maxAngle;
    private float spawningDistance = 10f;

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

        Vector3 spawnPosition = pos + directionFromCenter.normalized * (radius + spawningDistance);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - pos);

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, rotation, gameObject.transform);
        EnemyHealth enemyHealth = newEnemy.GetComponentInChildren<EnemyHealth>();
        
        if (enemyHealth != null)
        {
            enemyHealth.Killed += OnEnemyKill;
            enemyHealth.Killed += playerController.AddScore;
        }

        Enemy enemy = newEnemy.GetComponentInChildren<Enemy>();
        enemy.playerTransform = playerController.transform;
    }

    private Vector3 CalculateRandomVector3()
    {
        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);
        float randZ = Random.Range(-1f, 1f);

        return new Vector3(randX, randY, randZ);
    }

    public void OnEnemyKill(int score)
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
                if (angle <= maxAngle)
                {
                    return false;
                }
            }
            
        }
        return true;
    }

}
