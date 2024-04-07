using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes : MonoBehaviour
{
    const int threadGroupSize = 8;

    public MetaBalls metaBallGenerator;
    public ComputeShader shader;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    [Header("Voxel Settings")]
    public float isoLevel;
    public float spacing = 1;
    public Vector3 offset = Vector3.zero;

    [Range(2, 100)]
    public int numPointsPerAxis;

    // Buffers
    ComputeBuffer triangleBuffer;
    ComputeBuffer pointsBuffer;
    ComputeBuffer triCountBuffer;

    // sprobowaæ zrobiæ w tiku przyci¹ganie oraz tworzenie collidera
    // sprobowaæ zmienic przyci¹ganie na si³y metacz¹stek 31
    private void OnEnable()
    {
        //Ticker.OnTickAction += Tick;
    }

    private void OnDisable()
    {
        //Ticker.OnTickAction -= Tick;
    }

    // Start is called before the first frame update
    void Start()
    {
        metaBallGenerator.worldBounds = numPointsPerAxis;

        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    private void Update()
    {
        CreateBuffers();
        UpdateMesh();

        if (!Application.isPlaying)
        {
            ReleaseBuffers();
        }
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
    }

    void CreateBuffers()
    {
        int numPoints = numPointsPerAxis * numPointsPerAxis * numPointsPerAxis;
        int numVoxelsPerAxis = numPointsPerAxis - 1;
        int numVoxels = numVoxelsPerAxis * numVoxelsPerAxis * numVoxelsPerAxis;
        int maxTriangleCount = numVoxels * 5;

        // Always create buffers in editor (since buffers are released immediately to prevent memory leak)
        // Otherwise, only create if null or if size has changed
        if (!Application.isPlaying || (pointsBuffer == null || numPoints != pointsBuffer.count))
        {
            if (Application.isPlaying)
            {
                ReleaseBuffers();
            }
            triangleBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
            pointsBuffer = new ComputeBuffer(numPoints, sizeof(float) * 4);
            triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        }
    }

    public void UpdateMesh()
    {
        metaBallGenerator.Generate(pointsBuffer, numPointsPerAxis, spacing, offset);

        triangleBuffer.SetCounterValue(0);
        shader.SetBuffer(0, "points", pointsBuffer);
        shader.SetBuffer(0, "triangles", triangleBuffer);
        shader.SetInt("numPointsPerAxis", numPointsPerAxis);
        shader.SetFloat("isoLevel", isoLevel);
        int numVoxelsPerAxis = numPointsPerAxis - 1;
        int numThreadsPerAxis = Mathf.CeilToInt(numVoxelsPerAxis / (float)threadGroupSize);

        shader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        // Get number of triangles in the triangle buffer
        ComputeBuffer.CopyCount(triangleBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];

        // Get triangle data from shader
        Triangle[] tris = new Triangle[numTris];
        triangleBuffer.GetData(tris, 0, 0, numTris);

        // Create mesh and set data
        Mesh mesh = new Mesh();
        mesh.Clear();

        var vertices = new Vector3[numTris * 3];
        var meshTriangles = new int[numTris * 3];

        for (int i = 0; i < numTris; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                meshTriangles[i * 3 + j] = i * 3 + j;
                vertices[i * 3 + j] = tris[i][j];
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = meshTriangles;

        mesh.RecalculateNormals();

        // Destroy old meshes
        if (meshFilter.mesh != null)
        {
            Destroy(meshFilter.mesh);
        }
        if (meshCollider.sharedMesh != null)
        {
            Destroy(meshCollider.sharedMesh);
        }

        // Set mesh to game
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    private void Tick()
    {
        Debug.Log("DUPA");
        /*if (meshCollider.sharedMesh != null)
        {
            Destroy(meshCollider.sharedMesh);
        }
        meshCollider.sharedMesh = actualMesh;*/
    }

    private void ReleaseBuffers()
    {
        if (triangleBuffer != null)
        {
            triangleBuffer.Release();
            pointsBuffer.Release();
            triCountBuffer.Release();
        }
    }

    struct Triangle
    {
        #pragma warning disable 649 // disable unassigned variable warning
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Vector3 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    default:
                        return c;
                }
            }
        }
    }
}
