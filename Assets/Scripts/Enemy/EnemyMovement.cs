
using UnityEngine;

public class EnemyMovement : MovementController
{
    private LevelManager levelManager;
    [SerializeField] private float toPlayerDistance;

    protected override void Start()
    {
        base.Start();
        levelManager = LevelManager.Instance;
    }

    public override void MovementUpdate()
    {
        UpdateMovementDirection();
        base.MovementUpdate();
    }

    private void UpdateMovementDirection()
    {
        Vector3 toPlayerVector = levelManager.GetPlayerPosition() - transform.position;

        // If enemy is close then it stop moving
        if (toPlayerVector.magnitude <= toPlayerDistance)
        {
            movementDirection = Vector2.zero;
            return;
        }
        movementDirection = new Vector2(0, 1);
        Vector3 projectedVector = Vector3.ProjectOnPlane(toPlayerVector.normalized, transform.up);

        transform.rotation = Quaternion.FromToRotation(transform.forward, projectedVector.normalized)
                * transform.rotation;

        Debug.DrawRay(transform.position, projectedVector, Color.red);
    }

}
