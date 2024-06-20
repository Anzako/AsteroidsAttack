using UnityEngine;
using UnityEngine.UIElements;

public class Metaball : MonoBehaviour
{
    public Vector3 startDirection;
    public Vector3 direction;
    private Vector3 startPosition;

    public float speed;
    public float radius;

    private float marchingCubesSize;
    private Vector3 marchingCubesOffset;
    private static float marchingCubesInnerBoxDistance = 6f;

    private bool isBossFight = false;
    private Vector3 bossBoxOffset;
    private float bossBoxSize;

    private void Start()
    {
        direction = startDirection;
        startPosition = transform.position;
        marchingCubesSize = MarchingCubes.Instance.numPointsPerAxis;
        marchingCubesOffset = MarchingCubes.Instance.offset;
    }

    public void UpdateMetaball()
    {
        if (isBossFight)
        {
            CheckBoxCollision(bossBoxOffset, bossBoxSize, 0);
            UpdatePosition();
        } 
        else
        {
            float distanceToBorder = MetaBalls.CalculateActualRadius(this) + marchingCubesInnerBoxDistance;

            CheckBoxCollision(marchingCubesOffset, marchingCubesSize, distanceToBorder);
            UpdatePosition();
        }
    }

    private void CheckBoxCollision(Vector3 offset, float size, float distanceToBorder)
    {
        Vector3 position = transform.position;

        // X axis
        if (position.x - distanceToBorder < offset.x)
        {
            direction.x = Mathf.Abs(direction.x);
        }
        else if (position.x + distanceToBorder > offset.x + size)
        {
            direction.x = -Mathf.Abs(direction.x);
        }

        // Y axis
        if (position.y - distanceToBorder < offset.y)
        {
            direction.y = Mathf.Abs(direction.y);
        }
        else if (position.y + distanceToBorder > offset.y + size)
        {
            direction.y = -Mathf.Abs(direction.y);
        }

        // Z axis
        if (position.z - distanceToBorder < offset.z)
        {
            direction.z = Mathf.Abs(direction.z);
        }
        else if (position.z + distanceToBorder > offset.z + size)
        {
            direction.z = -Mathf.Abs(direction.z);
        }
    }

    private void UpdatePosition()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        float actualRadius = MetaBalls.CalculateActualRadius(this);
        Gizmos.DrawWireSphere(transform.position, actualRadius);
    }

    public bool IsInsideMarchingBox()
    {
        float actualRadius = MetaBalls.CalculateActualRadius(this);
        float distanceToBorder = actualRadius + marchingCubesInnerBoxDistance;

        return IsInsideBox(marchingCubesOffset, marchingCubesSize, distanceToBorder);
    }

    public bool IsInsideBox(Vector3 offset, float size, float distanceToBorder)
    {
        Vector3 position = transform.position;

        bool isInside = true;

        // X axis
        if (position.x - distanceToBorder < offset.x || position.x + distanceToBorder > offset.x + size)
        {
            isInside = false;
        }

        // Y axis
        if (position.y - distanceToBorder < offset.y || position.y + distanceToBorder > offset.y + size)
        {
            isInside = false;
        }

        // Z axis
        if (position.z - distanceToBorder < offset.z || position.z + distanceToBorder > offset.z + size)
        {
            isInside = false;
        }
      
        return isInside;
    }

    // On Boss Start
    public void MoveToBox(Vector3 bossBoxOffset, float bossBoxSize)
    {
        float center = bossBoxSize / 2;
        Vector3 centerPos = bossBoxOffset + new Vector3(center, center, center);
        direction = (centerPos - transform.position).normalized;

        this.bossBoxOffset = bossBoxOffset;
        this.bossBoxSize = bossBoxSize;

        isBossFight = true;
    }

    // On Boss End
    public void MoveInMarchingBox()
    {
        isBossFight = false;
        direction = Spawner.CalculateRandomVector3();
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
