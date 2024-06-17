using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject
{
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private EnemyMovement movementController;
    [SerializeField] private EnemyHealth healthController;

    // Shooting
    private float lastShootTime = 0;
    public float timeToShoot = 2.0f;
    public float shootRange;
    [SerializeField] private int projectileDamage = 2;
    [SerializeField] private float shootinRange;

    public int score;
    public int crashDamage;

    [SerializeField] private poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
    }

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

        if (toPlayerVector.magnitude < shootinRange)
        {
            if (lastShootTime > timeToShoot)
            {
                ShootProjectile();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(crashDamage);
            Destroy();
        }
    }

    public void Destroy()
    {
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
        ScoreManager.Instance.AddScore(score);
    }

    public void ShootProjectile()
    {
        GameObject projectile = Spawner.SpawnPoolObjectOnPosition(poolTags.enemyProjectile, projectileSpawnPoint.position, transform.rotation);
        projectile.GetComponent<Projectile>().damageAmount = projectileDamage;
        lastShootTime = 0;
    }

    public void OnObjectSpawn()
    {
        healthController.SetHealthToMax();
    }
}
