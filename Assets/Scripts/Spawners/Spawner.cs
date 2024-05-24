using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    private ObjectPooler pooler;
    private MetaBalls metaballs;
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        pooler = ObjectPooler.Instance;
        metaballs = MetaBalls.Instance;
    }

    // Pool Objects
    #region Pool Objects
    public GameObject SpawnPoolObjectOnPosition(poolTags objectTag, Vector3 position, Quaternion rotation)
    {
        return pooler.SpawnObject(objectTag, position, rotation);
    }

    public GameObject SpawnAwayFromPlayerView(poolTags objectTag, int maxAttempts = 20)
    {
        int randomMetaballID = Random.Range(0, MetaBalls.numberOfMetaballs);
        Vector3 spawnPosition = RandomPositionOnMetaball(randomMetaballID);
        Vector3 forceVector = MetaBalls.CalculateMetaballsNormal(spawnPosition);

        if (Vector3.Dot(forceVector, -playerTransform.up) < 0 && maxAttempts > 0)
        {
            return SpawnAwayFromPlayerView(objectTag, maxAttempts - 1);
        }

        if (maxAttempts <= 0)
        {
            Debug.LogWarning("Cannot find position away from player view.");
            return null;
        }

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - metaballs.Position(randomMetaballID));

        GameObject obj = pooler.SpawnObject(objectTag, spawnPosition, rotation);
        return obj;
    }

    public GameObject SpawnPoolObject(poolTags objectTag)
    {
        int randomMetaballID = Random.Range(0, MetaBalls.numberOfMetaballs);
        Vector3 spawnPosition = RandomPositionOnMetaball(randomMetaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - metaballs.Position(randomMetaballID));

        GameObject obj = pooler.SpawnObject(objectTag, spawnPosition, rotation);
        return obj;
    }

    public GameObject SpawnPoolObjectOnMetaball(poolTags objectTag, int metaballID)
    {
        Vector3 spawnPosition = RandomPositionOnMetaball(metaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - metaballs.Position(metaballID));
        
        GameObject obj = pooler.SpawnObject(objectTag, spawnPosition, rotation);
        return obj;
    }
    #endregion

    // Instance Objects
    #region Objects
    public GameObject SpawnGameObject(GameObject spawnObject, int metaballID)
    {
        Vector3 spawnPosition = RandomPositionOnMetaball(metaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - metaballs.Position(metaballID));
        return Instantiate(spawnObject, spawnPosition, rotation, this.gameObject.transform);
    }
    #endregion

    // Calculate
    public static Vector3 RandomPositionOnMetaball(int metaballID, int maxAttempts = 20)
    {
        MetaBalls metaballs = MetaBalls.Instance;

        float spawningDistance = 1.5f;
        Vector3 spawnPosition = metaballs.Position(metaballID) + CalculateRandomVector3() 
            * (MetaBalls.CalculateActualRadius(metaballs.metaballs[metaballID]) + spawningDistance);

        if (MetaBalls.CalculateScalarFieldValue(spawnPosition) > MarchingCubes.isoLevel && maxAttempts > 0)
        {
            return RandomPositionOnMetaball(metaballID, maxAttempts - 1);
        }

        if (maxAttempts <= 0)
        {
            Debug.LogWarning("Cannot calculate spawning position");
            return Vector3.zero;
        }

        return spawnPosition;
    }

    public static Vector3 CalculateRandomVector3()
    {
        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);
        float randZ = Random.Range(-1f, 1f);

        return new Vector3(randX, randY, randZ).normalized;
    }

}
