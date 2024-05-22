using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject
{
    [SerializeField] private Transform projectileSpawnPoint;
    private EnemyMovement movementController;
    private EnemyHealth healthController;
    private Spawner spawner;

    // Shooting
    private float lastShootTime = 0;
    public float timeToShoot = 2.0f;
    public float shootRange;

    [SerializeField] private poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
    }

    private void Start()
    {
        spawner = Spawner.Instance;
        movementController = GetComponent<EnemyMovement>();
        healthController = GetComponent<EnemyHealth>();
    }

    private void FixedUpdate()
    {
        movementController.MovementUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;

        if (lastShootTime > timeToShoot && movementController.isEnemyOnPlayerMetaball) 
        {
            ShootProjectile();
        }
    }



    public void ShootProjectile()
    {
        spawner.SpawnPoolObjectOnPosition(poolTags.enemyProjectile, projectileSpawnPoint.position, transform.rotation);
        lastShootTime = 0;
    }

    public void OnObjectSpawn()
    {
        
    }
}
