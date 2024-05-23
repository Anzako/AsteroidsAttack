using UnityEngine;

public class Metaball : MonoBehaviour
{
    public Vector3 startDirection;
    private Vector3 direction;
    private Vector3 startPosition;

    public float speed;
    public float radius;

    private void Start()
    {
        direction = startDirection;
        startPosition = transform.position;
    }

    public void UpdatePosition(Vector3 worldBounds)
    {
        float distanceToBorder = radius + 5;
        Vector3 position = transform.position;

        if (position.x - distanceToBorder < 0)
        {
            direction.x = Mathf.Abs(direction.x);
        } else if (position.x + distanceToBorder > worldBounds.x)
        {
            direction.x = -Mathf.Abs(direction.x);
        }

        if (position.y - distanceToBorder < 0)
        {
            direction.y = Mathf.Abs(direction.y);
        } else if (position.y + distanceToBorder > worldBounds.y)
        {
            direction.y = -Mathf.Abs(direction.y);
        }

        if (position.z - distanceToBorder < 0)
        {
            direction.z = Mathf.Abs(direction.z);
        } else if (position.z + distanceToBorder > worldBounds.z)
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

    public Vector3 Position 
    { 
        get { return transform.position; } 
    }
}
