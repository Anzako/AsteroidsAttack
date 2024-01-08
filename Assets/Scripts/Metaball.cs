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
        if (position.x - radius < 0 || position.x + radius > worldBounds.x)
        {
            direction.x = -direction.x;
        }

        if (position.y - radius < 0 || position.y + radius > worldBounds.y)
        {
            direction.y = -direction.y;
        }

        if (position.z - radius < 0 || position.z + radius > worldBounds.z)
        {
            direction.z = -direction.z;
        }

        position += direction * speed * Time.deltaTime;
    }
}
