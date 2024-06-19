using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MovementController
{
    private LevelManager levelManager;

    [SerializeField] private float stoppingDistanceToPlayer;

    private bool isOnPlayerMetaball;
    public bool closePlayerRange;

    protected override void Start()
    {
        base.Start();
        levelManager = LevelManager.Instance;
    }

    private void FixedUpdate()
    {
        Metaball playerMetaball = MetaBalls.GetContainingMetaball(levelManager.GetPlayerPosition());
        Metaball enemyMetaball = MetaBalls.GetContainingMetaball(transform.position);

        isOnPlayerMetaball = MetaBalls.AreMetaballsConnected(playerMetaball, enemyMetaball);
    }

    protected override void Update()
    {
        if (isOnPlayerMetaball)
        {
            UpdateMovementDirection();
        }

        base.Update();
    }

    private void UpdateMovementDirection()
    {
        Vector3 toPlayerVector = levelManager.GetPlayerPosition() - transform.position;


        // Enemy moving to player
        if (toPlayerVector.magnitude >= stoppingDistanceToPlayer)
        {
            movementDirection = new Vector2(0, 1);

            Vector3 projectedVector = Vector3.ProjectOnPlane(toPlayerVector.normalized, transform.up);
            Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, projectedVector.normalized)
                    * transform.rotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            closePlayerRange = false;
        }
        // Enemy in player range moving around player
        else
        {
            movementDirection = new Vector2(0, 0);
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            closePlayerRange = true;
        }

        //Debug.DrawRay(transform.position, projectedVector, Color.red);
    }
}
