using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MovementController
{
    private LineRenderer lineRenderer;
    [SerializeField] private int numberOfPoints = 10;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected override void Update()
    {

    }

    protected override void Start()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        float moveDistance = 1f;
        lineRenderer.positionCount = numberOfPoints;
        lineRenderer.SetPosition(0, transform.position);

        for (int i = 1; i < numberOfPoints; i++)
        {
            Move(moveDistance);
            for (int j = 0; j < 3; j++)
            {
                RotateToSurface2();
            }
            lineRenderer.SetPosition(i, transform.position);
        }
    }

    private void CheckCollision()
    {
        /*for (int i = 0;)
        Physics.Linecast()*/
    }
}
