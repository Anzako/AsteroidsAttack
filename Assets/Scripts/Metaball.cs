using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metaball : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    public Vector3 position;
    public float radius;

    public Metaball(Vector3 startPos, float radius, Vector3 dir, float speed)
    {
        this.position = startPos;
        this.radius = radius;
        this.direction = dir;
        this.speed = speed;
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

        position += direction * speed * Time.deltaTime;
    }
}
