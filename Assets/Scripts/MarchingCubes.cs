using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
 

public class MarchingCubes : MonoBehaviour
{
    [SerializeField] private int numberOfVerticesInAxis;
    //[SerializeField] private float sphereRadius;

    public List<List<List<float>>> marchingCubePoints;

    [SerializeField] MeshCreator _creator;

    Vector3 sphereCenter1 = new Vector3(7, 7, 7);
    [SerializeField] float sphereRadius1 = 4;
    Vector3 sphereCenter2 = new Vector3(12, 12, 11);
    [SerializeField] float sphereRadius2 = 4;

    private void Awake()
    {
        CreateVertices();
    }

    // Start is called before the first frame update
    void Start()
    {
        _creator.CreateMesh(numberOfVerticesInAxis, marchingCubePoints);
    }

    void Update()
    {
        sphereCenter1.x += 2 * Time.deltaTime;
        CreateVertices();
        _creator.CreateMesh(numberOfVerticesInAxis, marchingCubePoints);
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
                    float pointValue = CalculateMarchPointsValue(point);
                    marchingCubePoints[i][j].Add(pointValue);
                }
            }
        }
    }


    private float DistanceBetweenPoints(Vector3 a, Vector3 b)
    {
        float distance = (b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y) + (b.z - a.z) * (b.z - a.z);
        distance = Mathf.Sqrt(distance);

        return distance;
    } 

    private float CalculateMarchPointsValue(Vector3 vector)
    {
        float marchValue = CalculateSpherePointValue(vector, sphereCenter1, sphereRadius1);
        
        if (marchValue > CalculateSpherePointValue(vector, sphereCenter2, sphereRadius2))
        {
            marchValue = CalculateSpherePointValue(vector, sphereCenter2, sphereRadius2);
        }

        return marchValue;
    }

    private float CalculateSpherePointValue(Vector3 vector, Vector3 sphereCenter, float sphereRadius)
    {
        float distance = DistanceBetweenPoints(vector, sphereCenter);
        if (distance == sphereRadius)
        {
            return 0;
        }
        else if (distance > sphereRadius)
        {
            float value = distance - sphereRadius;
            value = (float)Math.Round(value, 2);
            if (value > 1)
            {
                value = 1;
            }
            return value;
        }
        else
        {
            float value = distance - sphereRadius;
            value = (float)Math.Round(value, 2);
            if (value < -1)
            {
                value = -1;
            }
            return value;
        }
    }


    public float GetMarchValue(int x, int y, int z)
    {
        return marchingCubePoints[x][y][z];
    }


}
