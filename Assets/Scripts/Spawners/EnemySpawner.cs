using UnityEngine;
using static ObjectPooler;

public class EnemySpawner : Singleton<EnemySpawner>
{
    public void SpawnEnemy(int level)
    {
        switch (level)
        {
            case 1:
                Spawner.SpawnAwayFromPlayerView(poolTags.ufoLVL1);
                break;
            case 2:
                Spawner.SpawnAwayFromPlayerView(poolTags.ufoLVL2);
                break;
        }

    }

    public void DestroyAllEnemies()
    {
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.ufoLVL1);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.ufoLVL2);
        ObjectPooler.Instance.ReturnObjectsToPool(poolTags.enemyProjectile);
    }

}
