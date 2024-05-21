using UnityEngine;

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
}
