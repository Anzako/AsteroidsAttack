using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        Vector3 spawnPosition = pos + directionFromCenter.normalized * (radius + spawningDistance);

        while (metaballs.CalculateScalarFieldValue(spawnPosition) > 0.5f)
        {
            Debug.Log("Zle2");
            directionFromCenter = CalculateRandomVector3();
            spawnPosition = pos + directionFromCenter.normalized * (radius / 2);
        }

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

}
