using UnityEngine;

[RequireComponent(typeof(DropItems))]
public class AsteroidController : MonoBehaviour, IPooledObject, IDropable
{
    #region Variables
    private AsteroidsSpawner spawner;
    [SerializeField] private AsteroidsHealth healthController;
    [SerializeField] private DropItems dropItems;
    [SerializeField] private poolTags particleTag;

    public int damageAmount;
    public int score;

    // Pooled object
    [SerializeField] private poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
    }
    #endregion

    private void Awake()
    {
        spawner = AsteroidsSpawner.Instance;

        healthController.Killed += OnProjectileDestroy;
    }

    private void OnDestroy()
    {
        healthController.Killed -= OnProjectileDestroy;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(damageAmount);
            ObjectPooler.Instance.SpawnObject(particleTag, transform.position, transform.rotation);
            Destroy();
        }
    }

    public void OnProjectileDestroy()
    {
        switch (Tag)
        {
            case poolTags.bigAsteroid:
                spawner.SpawnAsteroidOnDestroy(poolTags.mediumAsteroid, this.transform);
                spawner.SpawnAsteroidOnDestroy(poolTags.mediumAsteroid, this.transform);
                spawner.SpawnAsteroidOnDestroy(poolTags.mediumAsteroid, this.transform);
                break;
            case poolTags.mediumAsteroid:
                spawner.SpawnAsteroidOnDestroy(poolTags.healUpAsteroid, this.transform);
                spawner.SpawnAsteroidOnDestroy(poolTags.smallAsteroid, this.transform);
                break;
            default:
                break;
        }
        Destroy();
    }

    protected virtual void Destroy()
    {
        Drop();
        AsteroidsSpawner.Instance.OnAsteroidDestroy();
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
        ScoreManager.Instance.AddScore(score);
    }

    public void OnObjectSpawn()
    {
        healthController.SetHealthToMax();
    }

    public void Drop()
    {
        dropItems.DropItem();
    }
}
