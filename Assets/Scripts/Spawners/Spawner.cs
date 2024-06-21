using UnityEngine;

public class Spawner
{
    public static GameObject SpawnPoolObjectOnPosition(poolTags objectTag, Vector3 position, Quaternion rotation)
    {
        return ObjectPooler.Instance.SpawnObject(objectTag, position, rotation);
    }

    public static GameObject SpawnAwayFromPlayerView(poolTags objectTag, int maxAttempts = 30)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            int randomMetaballID = Random.Range(0, MetaBalls.numberOfMetaballs);
            Vector3 spawnPosition = RandomPositionOnMetaball(randomMetaballID);
            Vector3 forceVector = MetaBalls.CalculateMetaballsNormal(spawnPosition);

            // Check if the spawn position is away from the player's view
            Transform playerTransform = GameManager.GetPlayerController().transform;
            if (Vector3.Dot(forceVector, -playerTransform.up) >= 0)
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.Instance.Position(randomMetaballID));
                GameObject obj = ObjectPooler.Instance.SpawnObject(objectTag, spawnPosition, rotation);
                return obj;
            }
        }

        Debug.LogWarning("Cannot find position away from player view.");
        return null;
    }

    public static SpawnPosition RandomSpawnPositionOnMetaball(Metaball metaball, int maxAttempts = 40)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 spawnPosition = RandomPositionOnMetaball(metaball);
            Vector3 forceVector = MetaBalls.CalculateMetaballsNormal(spawnPosition);

            // Check if the spawn position is away from the player's view
            Transform playerTransform = GameManager.GetPlayerController().transform;
            if (Vector3.Dot(forceVector, -playerTransform.up) >= 0)
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - metaball.Position);
                return new SpawnPosition(spawnPosition, rotation);
            }
        }
        
        Debug.LogWarning("Cannot find position away from player view.");
        return null;
    }

    public static Vector3 RandomPositionOnMetaball(int metaballID, int maxAttempts = 10)
    {
        MetaBalls metaballs = MetaBalls.Instance;
        float spawningDistance = 5f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 spawnPosition = metaballs.Position(metaballID) + CalculateRandomVector3()
                * (MetaBalls.CalculateActualRadius(metaballs.GetMetaball(metaballID)) + spawningDistance);

            if (MetaBalls.CalculateScalarFieldValue(spawnPosition) < MarchingCubes.isoLevel)
            {
                return spawnPosition;
            }
        }

        return Vector3.zero;
    }

    public static Vector3 RandomPositionOnMetaball(Metaball metaball, int maxAttempts = 10)
    {
        MetaBalls metaballs = MetaBalls.Instance;
        float spawningDistance = 3f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 spawnPosition = metaball.Position + CalculateRandomVector3()
                * (MetaBalls.CalculateActualRadius(metaball) + spawningDistance);

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

public class SpawnPosition
{
    public Vector3 position;
    public Quaternion rotation;

    public SpawnPosition(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
