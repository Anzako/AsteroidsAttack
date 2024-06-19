
using UnityEngine;

public class EnemyMovement : MovementController
{
    private LevelManager levelManager;
    [SerializeField] private float toMinPlayerDistance;
    [SerializeField] private float toMaxPlayerDistance;
    private bool isEnemyOnPlayerMetaball;

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
        if (isEnemyOnPlayerMetaball)
        {
            UpdateMovementDirection();
        }
        
        base.Update();
    }

    private void UpdateMovementDirection()
    {
        Vector3 toPlayerVector = levelManager.GetPlayerPosition() - transform.position;

        actualSpeed = moveSpeed;

        // Enemy moving from player
        if (toPlayerVector.magnitude <= toMinPlayerDistance)
        {
            movementDirection = new Vector2(0, -1);
        } 
        // Enemy moving to player
        else if (toPlayerVector.magnitude >= toMaxPlayerDistance)
        {
            movementDirection = new Vector2(0, 1);
        } 
        // Enemy in player range moving around player
        else
        {
            movementDirection = new Vector2(1, 0);
            actualSpeed = moveSpeed / 2;
        }


        Vector3 projectedVector = Vector3.ProjectOnPlane(toPlayerVector.normalized, transform.up);

        transform.rotation = Quaternion.FromToRotation(transform.forward, projectedVector.normalized)
                * transform.rotation;

        //Debug.DrawRay(transform.position, projectedVector, Color.red);
    }

}
