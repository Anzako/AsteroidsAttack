using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarchingCubes : MonoBehaviour
{
    public int numberOfVerticesInAxis = 10;
    public int distanceBetweenVertices = 1;

    public List<List<List<int>>> _vertices;
    //[SerializeField] private GameObject vertexPrefab;
    //private GameObject _verticesParent;

    [SerializeField] MeshCreator _creator;

    private void Awake()
    {
        CreateVertices();
    }

    // Start is called before the first frame update
    void Start()
    {
        _creator.CreateMesh(numberOfVerticesInAxis);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateVertices()
    {
        _vertices = new List<List<List<int>>>();
        //_verticesParent = new GameObject("Vertices");
        float startingX = 0;


        for (int i = 0; i < numberOfVerticesInAxis; i++)
        {
            _vertices.Add(new List<List<int>>());

            for (int j = 0; j < numberOfVerticesInAxis; j++)
            {
                _vertices[i].Add(new List<int>());

                for (int k = 0; k < numberOfVerticesInAxis; k++)
                {
                    int randomValue = Random.Range(-2, 30);
                    _vertices[i][j].Add(randomValue);
                }
            }
        }
    }

    public int GetMarchValue(int x, int y, int z)
    {
        return _vertices[x][y][z];
    }


}
