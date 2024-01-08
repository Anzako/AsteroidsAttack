using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.Timeline;

public class MeshCreator : MonoBehaviour
{
    //private Mesh mesh;
    [SerializeField] OldMarchingCubes marchCube;
    private int[,] triangulation;
    private int[] cornerIndexFromEdgeA;
    private int[] cornerIndexFromEdgeB;
    private float isolevel = 0.5f;

    #region Mesh Variables
    private List<Vector3> _verticies = new List<Vector3>();
    private List<Vector2> uv = new List<Vector2>();
    private List<int> triangles = new List<int>();
    #endregion

    public struct Vertex
    {
        public int x; 
        public int y; 
        public int z;

        public Vertex(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public struct GridCell
    {
        public Vertex position;
        public float value;
    }

    private void Awake()
    {
        //mesh = new Mesh();
        triangulation = GetComponent<MarchingTables>().triangulation;
        cornerIndexFromEdgeA = GetComponent<MarchingTables>().cornerIndexAFromEdge;
        cornerIndexFromEdgeB = GetComponent<MarchingTables>().cornerIndexBFromEdge;
        
        _verticies = new List<Vector3>();
        uv = new List<Vector2>();
        triangles = new List<int>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreateMesh(int numberOfVerticesInAxis, List<List<List<float>>> marchingCubePoints)
    {
        for (int i = 0; i < numberOfVerticesInAxis - 1; i++)
        {
            for (int j = 0; j < numberOfVerticesInAxis - 1; j++)
            {
                for (int k = 0; k < numberOfVerticesInAxis - 1; k++)
                {
                    GridCell[] cells = CreateGridBox(i, j, k, marchingCubePoints);
                    AddGridMesh(cells);
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = _verticies.ToArray();
        //mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();


        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        _verticies.Clear();
        uv.Clear();
        triangles.Clear();
    }

    private void AddGridMesh(GridCell[] cells)
    {
        int[] grid = GetTriangulationArray(CalculatingCubeIndex(cells));
        Vector2[] uvs = new Vector2[3];
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);
        uvs[2] = new Vector2(0, 1);

        for (int i = 0; grid[i] != -1; i += 3)
        {
            for (int j = 2; j >= 0; j--)
            {
                int a = cornerIndexFromEdgeA[grid[i + j]];
                int b = cornerIndexFromEdgeB[grid[i + j]];
                Vector3 edgeVertex = EdgeInterp(isolevel, cells[a], cells[b]);

                _verticies.Add(edgeVertex);
                triangles.Add(_verticies.Count - 1);
                uv.Add(uvs[j]);
            }
        }
    }



    Vector3 EdgeInterp(float isolevel, GridCell a, GridCell b)
    {
        float mu;
        Vector3 p;
        GridCell c;
        if (a.value < b.value)
        {
            c = a;
            a = b; 
            b = c;
        }
        if (Mathf.Abs(isolevel - a.value) < 0.00001)
            return new Vector3(a.position.x, a.position.y, a.position.z);
        if (Mathf.Abs(isolevel - b.value) < 0.00001)
            return new Vector3(b.position.x, b.position.y, b.position.z);
        if (Mathf.Abs(a.value - b.value) < 0.00001)
            return new Vector3(a.position.x, a.position.y, a.position.z);
        mu = (isolevel - a.value) / (b.value - a.value);
        p.x = a.position.x + mu * (b.position.x - a.position.x);
        p.y = a.position.y + mu * (b.position.y - a.position.y);
        p.z = a.position.z + mu * (b.position.z - a.position.z);

        return(p);
    }

    Vector3 EdgeInterp2(float isolevel, GridCell b, GridCell a)
    {
        float mu = (isolevel - a.value) / (b.value - a.value);
        Vector3 vectorA = new Vector3(a.position.x, a.position.y, a.position.z);
        Vector3 vectorB = new Vector3(b.position.x, b.position.y, b.position.z);
        Vector3 p = Vector3.Lerp(vectorA, vectorB, mu);
        
        return (p);
    }




    private GridCell[] CreateGridBox(int x, int y, int z, List<List<List<float>>> marchingPoints)
    {
        GridCell[] gridBox = new GridCell[8];
        Vertex[] cubeIndexes = GetCubeCorners(new Vertex(x, y, z));

        for (int i = 0; i < 8; i++)
        {
            GridCell cell = new GridCell();
            cell.position = cubeIndexes[i];
            cell.value = marchingPoints[cell.position.x][cell.position.y][cell.position.z];

            gridBox[i] = cell;
        }

        return gridBox;
    }

    private int[] GetTriangulationArray(int number)
    {
        int[] triangulationArray = new int[16];
        for (int i = 0; i < 16; i++)
        {
            triangulationArray[i] = triangulation[number, i];
        }

        return triangulationArray;
    }

    private Vertex[] GetCubeCorners(Vertex startVector)
    {
        Vertex[] cubeCorners =
        {
            new Vertex (startVector.x , startVector.y, startVector.z),
            new Vertex (startVector.x + 1, startVector.y, startVector.z),
            new Vertex (startVector.x + 1, startVector.y, startVector.z + 1),
            new Vertex (startVector.x, startVector.y, startVector.z + 1),
            new Vertex (startVector.x, startVector.y + 1, startVector.z),
            new Vertex (startVector.x + 1, startVector.y + 1, startVector.z),
            new Vertex (startVector.x + 1, startVector.y + 1, startVector.z + 1),
            new Vertex (startVector.x, startVector.y + 1, startVector.z + 1)
        };

        return cubeCorners;
    }

    private int CalculatingCubeIndex(GridCell[] cells)
    {
        int cubeIndex = 0;

        if (cells[0].value < isolevel) cubeIndex |= 1;
        if (cells[1].value < isolevel) cubeIndex |= 2;
        if (cells[2].value < isolevel) cubeIndex |= 4;
        if (cells[3].value < isolevel) cubeIndex |= 8;
        if (cells[4].value < isolevel) cubeIndex |= 16;
        if (cells[5].value < isolevel) cubeIndex |= 32;
        if (cells[6].value < isolevel) cubeIndex |= 64;
        if (cells[7].value < isolevel) cubeIndex |= 128;

        return cubeIndex;
    }

}
