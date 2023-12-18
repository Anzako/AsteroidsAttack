using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBalls : MonoBehaviour
{
    const int threadGroupSize = 8;
    public ComputeShader metaballShader;

    public struct Metaball
    {
        public Vector3 centrePos;
        public float radius;
    }

    public Metaball[] metaballs;
    ComputeBuffer metaballBuffer;
    public float treshold = 4;

    public Vector3 vec = new Vector3(3, 3, 3);

    private void Start()
    {
        CreateMetaballs();
    }

    private void CreateMetaballs()
    {
        int numberOfMetaballs = 2;

        metaballs = new Metaball[numberOfMetaballs];
        for (int i = 0; i < numberOfMetaballs; i++)
        {
            metaballs[i] = new Metaball();
        }

        metaballs[0].centrePos = vec;
        metaballs[0].radius = 3;

        metaballs[1].centrePos = new Vector3(6, 6, 6);
        metaballs[1].radius = 3;
    }

    private void UpdateMetaballs()
    {
        metaballs[0].centrePos = vec;
        metaballs[0].radius = 3;

        metaballs[1].centrePos = new Vector3(6, 6, 6);
        metaballs[1].radius = 3;
    }

    //protected List<ComputeBuffer> buffersToRelease;
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
        metaballBuffer = new ComputeBuffer(metaballs.Length, sizeof(float) * 4);
        metaballBuffer.SetData(metaballs);
        metaballShader.SetBuffer(0, "metaballs", metaballBuffer);
        metaballShader.SetInt("numberOfMetaballs", metaballs.Length);
    }

}
