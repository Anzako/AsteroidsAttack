using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private List<Transform> projectilesSpawningPositions;
    [SerializeField] private Transform longAttackProjectileSpawnPosition;
    [SerializeField] private BossMovement movementController;
    [SerializeField] private EnemyHealth healthController;

    [Header("Shooting")]
    private float lastShootTime = 0;
    [SerializeField] private float shootingRange;
    [SerializeField] private int projectileDamage = 2;

    [Header("Close Range")]
    public float closeRangeShootTime;
    public float closeRangeTimeToDestroy;
    public float closeRangeSpeed;

    [Header("Long Range")]
    public float longRangeShootTime;
    public float longRangeTimeToDestroy;
    public float longRangeSpeed;

    public int score;
    public int crashDamage;

    private void OnDestroy()
    {
        healthController.Killed -= Destroy;
    }

    private void Start()
    {
        healthController.Killed += Destroy;
    }

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;
        Vector3 toPlayerVector = LevelManager.Instance.GetPlayerPosition() - transform.position;
        if (toPlayerVector.magnitude > shootingRange) return;


        if (movementController.closePlayerRange)
        {
            if (lastShootTime > closeRangeShootTime)
            {
                CloseDistanceAttack();
                lastShootTime = 0;
            }
        } else
        {
            if (lastShootTime > longRangeShootTime)
            {
                StartCoroutine(LongDistanceAttack());
                lastShootTime = 0;
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(crashDamage);
        }
    }

    public void Destroy()
    {
        ScoreManager.Instance.AddScore(score);
        Destroy(gameObject);
    }

    private void CloseDistanceAttack()
    {
        foreach(Transform t in projectilesSpawningPositions)
        {
            SpawnCloseRangeProjectile(t);
        }
    }

    private IEnumerator LongDistanceAttack()
    {
        SpawnLongRangeProjectile();
        yield return new WaitForSeconds(0.2f);
        SpawnLongRangeProjectile();
    }

    private void SpawnCloseRangeProjectile(Transform t)
    {
        Projectile projectile = Spawner.SpawnPoolObjectOnPosition(poolTags.enemyProjectile, 
            t.position, t.rotation).GetComponent<Projectile>();
        projectile.damageAmount = projectileDamage;
        projectile.timeToDestroy = closeRangeTimeToDestroy;
        projectile.SetSpeed(closeRangeSpeed);
    }

    private void SpawnLongRangeProjectile()
    {
        Projectile projectile = Spawner.SpawnPoolObjectOnPosition(poolTags.enemyProjectile, 
            longAttackProjectileSpawnPosition.position, longAttackProjectileSpawnPosition.rotation).GetComponent<Projectile>();
        projectile.damageAmount = projectileDamage;
        projectile.timeToDestroy = longRangeTimeToDestroy;
        projectile.SetSpeed(longRangeSpeed);

    }

    public void OnObjectSpawn()
    {
        healthController.SetHealthToMax();
    }
}
