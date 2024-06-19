using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Wave : ScriptableObject
{
    // Asteroids
    public int smallAsteroidAmount;
    public int mediumAsteroidAmount;
    public int bigAsteroidAmount;

    // Enemies
    public bool isEnemySpawning;
    public int spawningEnemiesAmount; 
    public float timeToSpawnEnemy;

    // Boss
    public bool isBoss;
    public GameObject bossGameObject;
}
