using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Player
    public Transform playerTransform;

    // Movement
    [SerializeField] private MovementController mController;
    [SerializeField] private EnemyHealth healthController;
    private Vector2 moveDirection = Vector2.zero;

    // Shooting
    [SerializeField] private GameObject projectile;
    private float lastShootTime = 0;
    public float timeToShoot = 2.0f;
    public float shootRange;

    private void FixedUpdate()
    {
        mController.MovementUpdate(moveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;

        if (lastShootTime > timeToShoot) 
        {
            float distance = (transform.position - playerTransform.position).magnitude;
            if (distance <= shootRange)
            {
                ShootProjectile();
            }
        }
    }

    public void ShootProjectile()
    {
        Vector3 spawnPosition = transform.position + transform.forward.normalized;
        spawnPosition += transform.up.normalized * 0.2f;
        Instantiate(projectile, spawnPosition, transform.rotation);
        lastShootTime = 0;
    }
}
