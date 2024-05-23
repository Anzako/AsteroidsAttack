using UnityEngine;

public class MetaBalls : Singleton<MetaBalls>
{
    const int threadGroupSize = 8;
    public ComputeShader metaballShader;
    [SerializeField] public Metaball[] metaballs;
    public static int numberOfMetaballs;
    public int worldBounds;

    public struct MetaballStruct
    {
        public Vector3 centrePos;
        public float radius;
    }

    public MetaballStruct[] metaballsStruct;
    private ComputeBuffer metaballBuffer;

    private void Start()
    {
        numberOfMetaballs = metaballs.Length;
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
            metaballsStruct[i].centrePos = metaballs[i].Position;
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
        return metaballs[ID].Position;
    }

    public float Radius(int ID)
    {
        return metaballs[ID].radius;
    }

    #region Calculations
    public static float CalculateScalarFieldValue(Vector3 position)
    {
        float scalarValue = 0.0f;

        for (int i = 0; i < numberOfMetaballs; i++)
        {
            scalarValue += CalculateMetaballScalarFieldValue(position, Instance.metaballs[i]);
        }
        return scalarValue;
    }

    public static float CalculateMetaballScalarFieldValue(Vector3 position, Metaball metaball)
    {
        float distance = distanceBetweenVectorsSq(position, metaball.Position);
        float scalarValue = metaball.radius / distance;
        return scalarValue;
    }

    public static Vector3 CalculateMetaballsNormal(Vector3 position)
    {
        Vector3 calculatedNormal = new Vector3();
        for (int i = 0; i < numberOfMetaballs; i++)
        {
            Vector3 normal = position - Instance.Position(i);
            normal = normal.normalized * CalculateMetaballScalarFieldValue(position, Instance.metaballs[i]);
            calculatedNormal += normal;
        }
        return calculatedNormal;
    }

    public static Metaball GetContainingMetaball(Vector3 pos)
    {
        Metaball containingMetaball = null;
        float maxValue = float.MinValue;

        foreach (var metaball in Instance.metaballs)
        {
            float distanceSq = distanceBetweenVectorsSq(metaball.Position, pos);
            if (distanceSq > 0)
            {
                float value = metaball.radius / distanceSq;
                if (value > maxValue)
                {
                    maxValue = value;
                    containingMetaball = metaball;
                }
            }
        }

        return containingMetaball;
    }

    public static bool AreMetaballsConnected(Metaball m1, Metaball m2)
    {
        int samplePoints = 10;

        for (int i = 1; i < samplePoints; i++)
        {
            float t = (float)i / samplePoints;
            Vector3 samplePoint = Vector3.Lerp(m1.Position, m2.Position, t);
            float valueAtSample = CalculateScalarFieldValue(samplePoint);

            if (valueAtSample < MarchingCubes.isoLevel)
            {
                return false;
            }
        }

        return true;
    }

    public static float CalculateActualRadius(Metaball metaball)
    {
        float radius = Mathf.Sqrt(metaball.radius / MarchingCubes.isoLevel);
        return radius;
    }

    private static float distanceBetweenVectorsSq(Vector3 a, Vector3 b)
    {
        float distance = (b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y) + (b.z - a.z) * (b.z - a.z);

        return distance;
    }
    #endregion
}
