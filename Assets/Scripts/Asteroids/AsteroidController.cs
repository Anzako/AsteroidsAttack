using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DropItems))]
public class AsteroidController : MonoBehaviour, IPooledObject, IDropable
{
    #region Variables
    private AsteroidsSpawner spawner;
    [SerializeField] private AsteroidsHealth healthController;
    [SerializeField] private DropItems dropItems;

    public int damageAmount;

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
        // Here spawn particle system
        Drop();
        AsteroidsSpawner.Instance.OnAsteroidDestroy();
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
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
