using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBalls : MonoBehaviour
{
    const int threadGroupSize = 8;
    public ComputeShader metaballShader;
    [SerializeField] private Metaball[] metaballs;
    private int numberOfMetaballs;

    public struct MetaballStruct
    {
        public Vector3 centrePos;
        public float radius;
    }

    public MetaballStruct[] metaballsStruct;
    ComputeBuffer metaballBuffer;
    public float treshold = 4;

    private void Start()
    {
        CreateMetaballs();
    }

    private void CreateMetaballs()
    {
        numberOfMetaballs = metaballs.Length;

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
            metaball.UpdatePosition(new Vector3(30, 30, 30));
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
        int numPoints = numPointsPerAxis * numPointsPerAxis * numPointsPerAxis;
        int numThreadsPerAxis = Mathf.CeilToInt(numPointsPerAxis / (float)threadGroupSize);
        // Points buffer is populated inside shader with pos (xyz) + density (w).
        // Set paramaters

        UpdateMetaballs();
        CreateBuffers();
        metaballShader.SetBuffer(0, "points", pointsBuffer);
        metaballShader.SetInt("numPointsPerAxis", numPointsPerAxis);
        metaballShader.SetFloat("spacing", spacing);
        metaballShader.SetVector("offset", offset);
        metaballShader.SetFloat("treshold", treshold);

        // Dispatch shader
        metaballShader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        metaballBuffer.Release();

        // Return voxel data buffer so it can be used to generate mesh
        return pointsBuffer;
    }

    private void CreateBuffers()
    {
        metaballBuffer = new ComputeBuffer(metaballsStruct.Length, sizeof(float) * 4);
        metaballBuffer.SetData(metaballsStruct);
        metaballShader.SetBuffer(0, "metaballs", metaballBuffer);
        metaballShader.SetInt("numberOfMetaballs", metaballsStruct.Length);
    }

}
