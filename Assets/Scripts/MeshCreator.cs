using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.Timeline;

public class MeshCreator : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField] MarchingCubes marchCube;
    private int[,] triangulation;
    private int[] cornerIndexFromEdgeA;
    private int[] cornerIndexFromEdgeB;
    private int isolevel = 0;

    #region Mesh Variables
    private List<Vector3> _verticies = new List<Vector3>();
    private List<Vector2> uv = new List<Vector2>();
    private List<int> triangles = new List<int>();
    #endregion

    public struct GridCell
    {
        public Vector3 position;
        public float value;
    }

    private void Awake()
    {
        mesh = new Mesh();
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

    public void CreateMesh(int numberOfVerticesInAxis)
    {
        for (int i = 0; i < numberOfVerticesInAxis - 1; i++)
        {
            for (int j = 0; j < numberOfVerticesInAxis - 1; j++)
            {
                for (int k = 0; k < numberOfVerticesInAxis - 1; k++)
                {
                    GridCell[] cells = CreateGridBox(i, j, k);
                    AddGridMesh(cells);
                }
            }
        }

        mesh.vertices = _verticies.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;
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
            for (int j = 0; j < 3; j++)
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



    Vector3 EdgeInterp(int isolevel, GridCell a, GridCell b)
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
            return(a.position);
        if (Mathf.Abs(isolevel - b.value) < 0.00001)
            return(b.position);
        if (Mathf.Abs(a.value - b.value) < 0.00001)
            return(a.position);
        mu = (isolevel - a.value) / (b.value - a.value);
        p.x = a.position.x + mu * (b.position.x - a.position.x);
        p.y = a.position.y + mu * (b.position.y - a.position.y);
        p.z = a.position.z + mu * (b.position.z - a.position.z);

        return(p);
    }
    


    private GridCell[] CreateGridBox(int x, int y, int z)
    {
        GridCell[] gridBox = new GridCell[8];
        Vector3[] cubeIndexes = GetCubeCorners(new Vector3(x, y, z));

        for (int i = 0; i < 8; i++)
        {
            GridCell cell = new GridCell();
            cell.position = cubeIndexes[i];
            cell.value = marchCube.GetMarchValue((int)cell.position.x, (int)cell.position.y, (int)cell.position.z);

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

    private Vector3[] GetCubeCorners(Vector3 startVector)
    {
        Vector3[] cubeCorners =
        {
            new Vector3 (startVector.x , startVector.y, startVector.z),
            new Vector3 (startVector.x + 1, startVector.y, startVector.z),
            new Vector3 (startVector.x + 1, startVector.y, startVector.z + 1),
            new Vector3 (startVector.x, startVector.y, startVector.z + 1),
            new Vector3 (startVector.x, startVector.y + 1, startVector.z),
            new Vector3 (startVector.x + 1, startVector.y + 1, startVector.z),
            new Vector3 (startVector.x + 1, startVector.y + 1, startVector.z + 1),
            new Vector3 (startVector.x, startVector.y + 1, startVector.z + 1)
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

    // Update is called once per frame
    void Update()
    {

    }
}
