using UnityEngine;
using static ObjectPooler;

public class EnemySpawner : Singleton<EnemySpawner>
{
    private Spawner spawner;
    [SerializeField] private poolTags enemyTag;

    void Start()
    {
        spawner = Spawner.Instance;
    }

    public void SpawnEnemy()
    {
        spawner.SpawnAwayFromPlayerView(enemyTag);
    }

    public void DestroyAllEnemies()
    {
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.enemy);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.enemyProjectile);
    }

}
