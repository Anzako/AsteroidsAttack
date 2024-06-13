using UnityEngine;
using static ObjectPooler;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private poolTags enemyTag;

    public void SpawnEnemy()
    {
        Spawner.SpawnAwayFromPlayerView(enemyTag);
    }

    public void DestroyAllEnemies()
    {
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.enemy);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.enemyProjectile);
    }

}
