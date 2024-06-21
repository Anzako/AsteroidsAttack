using System.Collections.Generic;
using UnityEngine;

public class MetaBalls : Singleton<MetaBalls>
{
    const int threadGroupSize = 8;
    public ComputeShader metaballShader;
    private List<Metaball> metaballs;
    public Metaball startMetaball1;
    public Metaball startMetaball2;
    public static int numberOfMetaballs;

    public Transform bossBoxTransform;
    public float bossBoxSize;

    public struct MetaballStruct
    {
        public Vector3 centrePos;
        public float radius;
    }

    public MetaballStruct[] metaballsStruct;
    private ComputeBuffer metaballBuffer;

    private void Start()
    {
        metaballs = new List<Metaball>
        {
            startMetaball1,
            startMetaball2
        };
        CreateMetaballs();
    }

    private void UpdateMetaballs()
    {
        foreach (Metaball metaball in metaballs)
        {
            metaball.UpdateMetaball();
        }
        UpdateMetaballsStruct();
    }

    #region Marching Cubes Staff
    private void CreateMetaballs()
    {
        numberOfMetaballs = metaballs.Count;
        metaballsStruct = new MetaballStruct[numberOfMetaballs];
        for (int i = 0; i < numberOfMetaballs; i++)
        {
            metaballsStruct[i] = new MetaballStruct();
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
    #endregion

    public Vector3 Position(int ID)
    {
        return metaballs[ID].Position;
    }

    public float Radius(int ID)
    {
        return metaballs[ID].radius;
    }

    public void ResetMetaballsParameters()
    {
        foreach (Metaball metaball in metaballs)
        {
            metaball.ResetParameters();
        }
        metaballs = new List<Metaball>
        {
            startMetaball1,
            startMetaball2
        };
        CreateMetaballs();
    }

    public void AddMetaball(Metaball metaball)
    {
        metaballs.Add(metaball);
        CreateMetaballs();
    }

    // On Boss Start
    public void OnBossStart()
    {
        foreach (Metaball metaball in metaballs)
        {
            metaball.MoveToBox(bossBoxTransform.position, bossBoxSize);
        }
    }

    // On Boss End
    public void OnBossEnd()
    {
        foreach (Metaball metaball in metaballs)
        {
            metaball.MoveInMarchingBox();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        
        float center = bossBoxSize / 2;

        Vector3 centerPos = bossBoxTransform.position + new Vector3(center, center, center);
        Gizmos.DrawWireCube(centerPos, new Vector3(bossBoxSize, bossBoxSize, bossBoxSize));
    }

    public Metaball GetMetaball(int id)
    {
        return metaballs[id];
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
