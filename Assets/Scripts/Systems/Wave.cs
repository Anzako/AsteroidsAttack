using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Wave : ScriptableObject
{
    // Asteroids
    [Range(1, 2)]
    public int asteroidLevel;
    public int smallAsteroidAmount;
    public int mediumAsteroidAmount;
    public int bigAsteroidAmount;

    // Enemies
    public bool isEnemySpawning;
    [Range(1, 2)]
    public int enemyLevel;
    public int spawningEnemiesAmount; 
    public float timeToSpawnEnemy;

    // Boss
    public bool isBoss;
    public GameObject bossGameObject;
}
