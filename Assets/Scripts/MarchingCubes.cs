using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarchingCubes : MonoBehaviour
{
    [SerializeField] private int numberOfVerticesInAxis;
    [SerializeField] private float sphereRadius;

    public List<List<List<float>>> marchingCubePoints;

    [SerializeField] MeshCreator _creator;

    private void Awake()
    {
        CreateVertices();
    }

    // Start is called before the first frame update
    void Start()
    {
        _creator.CreateMesh(numberOfVerticesInAxis);
    }

    private void CreateVertices()
    {
        marchingCubePoints = new List<List<List<float>>>();

        for (int i = 0; i < numberOfVerticesInAxis; i++)
        {
            marchingCubePoints.Add(new List<List<float>>());

            for (int j = 0; j < numberOfVerticesInAxis; j++)
            {
                marchingCubePoints[i].Add(new List<float>());

                for (int k = 0; k < numberOfVerticesInAxis; k++)
                {
                    Vector3 point = new Vector3(i, j, k);
                    float pointValue = CalculateMarchPointValue(point);
                    marchingCubePoints[i][j].Add(pointValue);
                }
            }
        }
    }

    private Vector3 GetSphereCentrePoint()
    {
        int point = numberOfVerticesInAxis / 2;
        Vector3 center = new Vector3(point, point, point);

        return center;
    }

    private float DistanceBetweenPoints(Vector3 a, Vector3 b)
    {
        float distance = (b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y) + (b.z - a.z) * (b.z - a.z);
        distance = Mathf.Sqrt(distance);

        return distance;
    } 

    private float CalculateMarchPointValue(Vector3 v)
    {
        float distance = DistanceBetweenPoints(v, GetSphereCentrePoint());
        float value = 0;
        if (distance > sphereRadius)
        {
            value = 1;
            if (distance - sphereRadius < 1)
            {
                value = (distance - sphereRadius);
            }
        } 
        else if (distance < sphereRadius)
        {
            value = (distance - sphereRadius);
        }

        return value;
    }


    public float GetMarchValue(int x, int y, int z)
    {
        return marchingCubePoints[x][y][z];
    }


}
