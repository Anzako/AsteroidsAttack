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

    [SerializeField] Vector3 sphereCenter1 = new Vector3(7, 7, 7);
    [SerializeField] float sphereRadius1 = 4;
    Vector3 sphereCenter2 = new Vector3(12, 12, 11);
    [SerializeField] float sphereRadius2 = 4;

    public float treshold = 2f;

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
        float sphereSpeed = 2;

        /*if (sphereCenter1.x > 10)
        {
            sphereSpeed = -sphereSpeed;
        }*/
        //sphereCenter1.x += sphereSpeed * Time.deltaTime;
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

    private float CalculateMarchPointsValue(Vector3 point)
    {

        float distance = DistanceBetweenPoints(sphereCenter1, point);
        double marchValue = 0;
        if (distance > (sphereRadius1 + treshold))
        {
            marchValue = 0;
        } else
        {
            marchValue += Math.Exp(-(distance * distance) / (sphereRadius1 * sphereRadius1));
        }
        
        distance = DistanceBetweenPoints(sphereCenter2, point);
        if (distance > (sphereRadius1 + treshold))
        {
            marchValue += 0;
        } else
        {
            marchValue += Math.Exp(-(distance * distance) / (sphereRadius2 * sphereRadius2));
        }

        /*float marchValue = CalculateSpherePointValue(point, sphereCenter1, sphereRadius1);
        
        if (marchValue > CalculateSpherePointValue(point, sphereCenter2, sphereRadius2))
        {
            marchValue = CalculateSpherePointValue(point, sphereCenter2, sphereRadius2);
        }*/

        return (float)marchValue;
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
            return value;
        }
        else
        {
            float value = distance - sphereRadius;
            value = (float)Math.Round(value, 2);
            return value;
        }
    }


    public float GetMarchValue(int x, int y, int z)
    {
        return marchingCubePoints[x][y][z];
    }


}
