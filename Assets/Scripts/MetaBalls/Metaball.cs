using UnityEngine;

public class Metaball : MonoBehaviour
{
    public Vector3 startDirection;
    private Vector3 direction;
    public Vector3 startPosition;
    private Vector3 position;

    public float speed;
    public float radius;

    private void Start()
    {
        direction = startDirection;
        position = startPosition;
    }

    public void UpdatePosition(Vector3 worldBounds)
    {
        float distanceToBorder = radius + 5;
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

        position += speed * Time.deltaTime * direction;
    }

    public Vector3 Position 
    { 
        get { return position; } 
    }
}
