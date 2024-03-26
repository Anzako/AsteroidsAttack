using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MetaBalls : MonoBehaviour
{
    public static MetaBalls instance { get; private set; }

    const int threadGroupSize = 8;
    public ComputeShader metaballShader;
    [SerializeField] private Metaball[] metaballs;
    public int numberOfMetaballs;
    public int worldBounds;

    public struct MetaballStruct
    {
        public Vector3 centrePos;
        public float radius;
    }

    public MetaballStruct[] metaballsStruct;
    ComputeBuffer metaballBuffer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        numberOfMetaballs = metaballs.Length;
    }

    private void Start()
    {
        CreateMetaballs();
    }

    private void CreateMetaballs()
    {
        metaballsStruct = new MetaballStruct[numberOfMetaballs];
        for (int i = 0; i < numberOfMetaballs; i++)
        {
            metaballsStruct[i] = new MetaballStruct();
        }

        UpdateMetaballsStruct();
    }

    private void UpdateMetaballs()
    {
        foreach (Metaball metaball in metaballs)
        {
            metaball.UpdatePosition(new Vector3(worldBounds, worldBounds, worldBounds));
        }
        UpdateMetaballsStruct();
    }

    private void UpdateMetaballsStruct()
    {
        for (int i = 0; i < numberOfMetaballs; i++)
        {
            metaballsStruct[i].centrePos = metaballs[i].position;
            metaballsStruct[i].radius = metaballs[i].radius;
        }
    }

    // Start is called before the first frame update
    public ComputeBuffer Generate(ComputeBuffer pointsBuffer, int numPointsPerAxis, float spacing, Vector3 offset)
    {
        int numThreadsPerAxis = Mathf.CeilToInt(numPointsPerAxis / (float)threadGroupSize);

        UpdateMetaballs();
        CreateBuffers();
        metaballShader.SetBuffer(0, "points", pointsBuffer);
        metaballShader.SetInt("numPointsPerAxis", numPointsPerAxis);
        metaballShader.SetFloat("spacing", spacing);
        metaballShader.SetVector("offset", offset);

        metaballShader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        metaballBuffer.Release();

        return pointsBuffer;
    }

    private void CreateBuffers()
    {
        metaballBuffer = new ComputeBuffer(numberOfMetaballs, sizeof(float) * 4);
        metaballBuffer.SetData(metaballsStruct);
        metaballShader.SetBuffer(0, "metaballs", metaballBuffer);
        metaballShader.SetInt("numberOfMetaballs", numberOfMetaballs);
    }

    public Vector3 Position(int ID)
    {
        return metaballs[ID].position;
    }

    public float Radius(int ID)
    {
        return metaballs[ID].radius;
    }

    public float CalculateScalarFieldValue(Vector3 position)
    {
        float scalarValue = 0.0f;

        for (int i = 0; i < numberOfMetaballs; i++)
        {
            scalarValue += CalculateMetaballScalarFieldValue(position, metaballs[i]);
        }
        return scalarValue;
    }

    public float CalculateMetaballScalarFieldValue(Vector3 position, Metaball metaball)
    {
        float distance = distanceBetweenVectorsSq(position, metaball.position);
        float scalarValue = metaball.radius / distance;
        return scalarValue;
    }

    public Vector3 CalculateMetaballsNormal(Vector3 position)
    {
        Vector3 calculatedNormal = new Vector3();
        for (int i = 0; i < numberOfMetaballs; i++)
        {
            Vector3 normal = position - metaballs[i].position;
            normal = normal.normalized * CalculateMetaballScalarFieldValue(position, metaballs[i]);
            calculatedNormal += normal;
        }
        return calculatedNormal;
    }

    private float distanceBetweenVectorsSq(Vector3 a, Vector3 b)
    {
        float distance = (b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y) + (b.z - a.z) * (b.z - a.z);

        return distance;
    }

}
