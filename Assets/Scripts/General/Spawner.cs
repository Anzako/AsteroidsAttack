using UnityEngine;

public class Spawner : MonoBehaviour
{
    private ObjectPooler pooler;
    private MetaBalls metaballs;

    private void Start()
    {
        pooler = ObjectPooler.instance;
        metaballs = MetaBalls.instance;
    }

    // Pool Objects
    #region Pool Objects
    public GameObject SpawnPoolObjectOnPosition(string objectTag, Vector3 position, Quaternion rotation)
    {
        return pooler.SpawnObject(objectTag, position, rotation);
    }

    public GameObject SpawnAwayFromPlayerView(string objectTag, Vector3 cameraDir, int maxAttempts = 10)
    {
        int randomMetaballID = Random.Range(0, metaballs.numberOfMetaballs);
        Vector3 spawnPosition = RandomPositionOnMetaball(randomMetaballID);
        Vector3 forceVector = metaballs.CalculateMetaballsNormal(spawnPosition);

        if (Vector3.Dot(forceVector, cameraDir) < 0 && maxAttempts > 0)
        {
            return SpawnAwayFromPlayerView(objectTag, cameraDir, maxAttempts - 1);
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

    public GameObject SpawnPoolObject(string objectTag)
    {
        int randomMetaballID = Random.Range(0, metaballs.numberOfMetaballs);
        Vector3 spawnPosition = RandomPositionOnMetaball(randomMetaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - metaballs.Position(randomMetaballID));

        GameObject obj = pooler.SpawnObject(objectTag, spawnPosition, rotation);
        return obj;
    }

    public GameObject SpawnPoolObjectOnMetaball(string objectTag, int metaballID)
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
    public static Vector3 RandomPositionOnMetaball(int metaballID, int maxAttempts = 10)
    {
        MetaBalls metaballs = MetaBalls.instance;

        Vector3 pos = metaballs.Position(metaballID);
        float radius = metaballs.Radius(metaballID);
        Vector3 spawnPosition = pos + CalculateRandomVector3() * (radius / 2);

        while (metaballs.CalculateScalarFieldValue(spawnPosition) > 0.5f && maxAttempts > 0)
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
