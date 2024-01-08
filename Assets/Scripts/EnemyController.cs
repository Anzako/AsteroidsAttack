using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private MovementController mController;
    public int health = 10;

    private float lastShootTime = 0;
    public Vector2 moveDirection = Vector2.zero;

    // Prefabs
    [SerializeField] private GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        mController.MovementFixedUpdate(moveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;

        if (lastShootTime > 2) 
        {
            //ShootProjectile();
            lastShootTime = 0;
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Debug.Log("Enemy dead");
            Destroy(gameObject);
        }
        
    }

    public void ShootProjectile()
    {
        Vector3 spawnPosition = transform.position + transform.forward.normalized;
        spawnPosition += transform.up.normalized * 1;
        Instantiate(projectile, spawnPosition, transform.rotation);
    }
}
