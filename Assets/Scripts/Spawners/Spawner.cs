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
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            int randomMetaballID = Random.Range(0, MetaBalls.numberOfMetaballs);
            Vector3 spawnPosition = RandomPositionOnMetaball(randomMetaballID);
            Vector3 forceVector = MetaBalls.CalculateMetaballsNormal(spawnPosition);

            // Check if the spawn position is away from the player's view
            if (Vector3.Dot(forceVector, -playerTransform.up) >= 0)
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - metaballs.Position(randomMetaballID));
                GameObject obj = pooler.SpawnObject(objectTag, spawnPosition, rotation);
                return obj;
            }
        }

        Debug.LogWarning("Cannot find position away from player view.");
        return null;
    }

    #endregion

    // Instance Objects
    #region Objects
    public GameObject SpawnGameObjectOnMetaball(GameObject spawnObject, int metaballID)
    {
        Vector3 spawnPosition = RandomPositionOnMetaball(metaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - metaballs.Position(metaballID));

        return Instantiate(spawnObject, spawnPosition, rotation, gameObject.transform);
    }
    #endregion

    public static Vector3 RandomPositionOnMetaball(int metaballID, int maxAttempts = 10)
    {
        MetaBalls metaballs = MetaBalls.Instance;
        float spawningDistance = 1.5f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 spawnPosition = metaballs.Position(metaballID) + CalculateRandomVector3()
                * (MetaBalls.CalculateActualRadius(metaballs.metaballs[metaballID]) + spawningDistance);

            if (MetaBalls.CalculateScalarFieldValue(spawnPosition) < MarchingCubes.isoLevel)
            {
                return spawnPosition;
            }
        }

        return Vector3.zero;
    }

    public static Vector3 CalculateRandomVector3()
    {
        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);
        float randZ = Random.Range(-1f, 1f);

        return new Vector3(randX, randY, randZ).normalized;
    }

}
