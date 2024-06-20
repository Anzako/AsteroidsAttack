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

}
