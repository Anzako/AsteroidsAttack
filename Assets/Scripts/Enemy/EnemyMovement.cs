
using UnityEngine;

public class EnemyMovement : MovementController
{
    private LevelManager levelManager;
    [SerializeField] private float toPlayerDistance;
    public bool isEnemyOnPlayerMetaball;

    protected override void Start()
    {
        base.Start();
        levelManager = LevelManager.Instance;
    }

    private void FixedUpdate()
    {
        Metaball playerMetaball = MetaBalls.GetContainingMetaball(levelManager.GetPlayerPosition());
        Metaball enemyMetaball = MetaBalls.GetContainingMetaball(transform.position);

        isEnemyOnPlayerMetaball = MetaBalls.AreMetaballsConnected(playerMetaball, enemyMetaball);
    }

    protected override void Update()
    {
        UpdateMovementDirection();
        base.Update();
    }

    private void UpdateMovementDirection()
    {
        if (!isEnemyOnPlayerMetaball) return;

        Vector3 toPlayerVector = levelManager.GetPlayerPosition() - transform.position;
        movementDirection = new Vector2(0, 1);

        // If enemy is close then it stop moving
        if (toPlayerVector.magnitude <= toPlayerDistance)
        {
            movementDirection = Vector2.zero;
        }

        Vector3 projectedVector = Vector3.ProjectOnPlane(toPlayerVector.normalized, transform.up);

        transform.rotation = Quaternion.FromToRotation(transform.forward, projectedVector.normalized)
                * transform.rotation;

        //Debug.DrawRay(transform.position, projectedVector, Color.red);
    }

}
