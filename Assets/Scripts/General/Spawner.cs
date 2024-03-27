using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;

    public GameObject SpawnGameObject(int metaballID)
    {
        Vector3 spawnPosition = SpawnPosition(metaballID);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnPosition - MetaBalls.instance.Position(metaballID));
        return Instantiate(spawnObject, spawnPosition, rotation);
    }

    public static Vector2 SpawnPosition(int metaballID)
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

    private static Vector3 CalculateRandomVector3()
    {
        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);
        float randZ = Random.Range(-1f, 1f);

        return new Vector3(randX, randY, randZ).normalized;
    }

}
