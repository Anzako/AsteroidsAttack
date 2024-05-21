using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject
{
    // Movement
    [SerializeField] private Transform projectileSpawnPoint;
    private MovementController movementController;
    private EnemyHealth healthController;
    private Vector2 moveDirection = Vector2.zero;

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
        movementController = GetComponent<MovementController>();
        healthController = GetComponent<EnemyHealth>();
    }

    private void FixedUpdate()
    {
        movementController.MovementUpdate(moveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;

        if (lastShootTime > timeToShoot) 
        {
            ShootProjectile();
        }
    }

    public void ShootProjectile()
    {
        ObjectPooler.Instance.SpawnObject(poolTags.enemyProjectile, projectileSpawnPoint.position, transform.rotation);
        lastShootTime = 0;
    }

    public void OnObjectSpawn()
    {
        
    }
}
