using UnityEngine;

public class Metaball : MonoBehaviour
{
    public Vector3 startDirection;
    public Vector3 direction;
    private Vector3 startPosition;

    public float speed;
    public float radius;

    private float marchingCubesSize;
    private static float marchingCubesInnerBoxDistance = 4f;

    private void Start()
    {
        direction = startDirection;
        startPosition = transform.position;
        marchingCubesSize = MarchingCubes.Instance.numPointsPerAxis;
    }

    public void UpdatePosition()
    {
        float actualRadius = MetaBalls.CalculateActualRadius(this);
        float distanceToBorder = actualRadius + marchingCubesInnerBoxDistance;
        Vector3 position = transform.position;
        
        // X axis
        if (position.x - distanceToBorder < 0)
        {
            direction.x = Mathf.Abs(direction.x);
        } else if (position.x + distanceToBorder > marchingCubesSize)
        {
            direction.x = -Mathf.Abs(direction.x);
        }

        // Y axis
        if (position.y - distanceToBorder < 0)
        {
            direction.y = Mathf.Abs(direction.y);
        } else if (position.y + distanceToBorder > marchingCubesSize)
        {
            direction.y = -Mathf.Abs(direction.y);
        }

        // Z axis
        if (position.z - distanceToBorder < 0)
        {
            direction.z = Mathf.Abs(direction.z);
        } else if (position.z + distanceToBorder > marchingCubesSize)
        {
            direction.z = -Mathf.Abs(direction.z);
        }

        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        float actualRadius = MetaBalls.CalculateActualRadius(this);
        Gizmos.DrawWireSphere(transform.position, actualRadius);
    }

    public void ResetParameters()
    {
        transform.position = startPosition;
        direction = startDirection;
    }

    public Vector3 Position 
    { 
        get { return transform.position; } 
    }
}
