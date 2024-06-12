using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject
{
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private EnemyMovement movementController;
    [SerializeField] private EnemyHealth healthController;
    private Spawner spawner;

    // Shooting
    private float lastShootTime = 0;
    public float timeToShoot = 2.0f;
    public float shootRange;

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
        spawner = Spawner.Instance;
        healthController.Killed += Destroy;
    }

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;

        if (lastShootTime > timeToShoot && movementController.canShoot) 
        {
            ShootProjectile();
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
        spawner.SpawnPoolObjectOnPosition(poolTags.enemyProjectile, projectileSpawnPoint.position, transform.rotation);
        lastShootTime = 0;
    }

    public void OnObjectSpawn()
    {
        healthController.SetHealthToMax();
    }
}
