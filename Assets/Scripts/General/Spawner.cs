using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    private ObjectPooler pooler;

    private void Start()
    {
        pooler = ObjectPooler.instance;
    }

    public GameObject SpawnGameObject(string objectTag, Vector3 position, Quaternion rotation)
    {
        GameObject obj = pooler.SpawnObject(objectTag, position, rotation);
        return obj;
    }

    public GameObject SpawnGameObject(int metaballID, string objectTag)
    {
        Vector3 spawnPosition = SpawnPosition(metaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.instance.Position(metaballID));
        
        GameObject obj = pooler.SpawnObject(objectTag, spawnPosition, rotation);
        return obj;
    }

    public GameObject SpawnGameObject(int metaballID)
    {
        Vector3 spawnPosition = SpawnPosition(metaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.instance.Position(metaballID));
        return Instantiate(spawnObject, spawnPosition, rotation, this.gameObject.transform);
    }

    public static Vector3 SpawnPosition(int metaballID)
    {
        MetaBalls metaballs = MetaBalls.instance;

        Vector3 pos = metaballs.Position(metaballID);
        float radius = metaballs.Radius(metaballID);

        Vector3 spawnPosition = pos + CalculateRandomVector3() * (radius / 2);

        while (metaballs.CalculateScalarFieldValue(spawnPosition) > 0.5f)
        {
            spawnPosition = pos + CalculateRandomVector3() * (radius / 2);
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
