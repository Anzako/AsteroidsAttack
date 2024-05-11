using System;
using System.Collections;
using UnityEngine;

public class AsteroidController : MonoBehaviour, IPooledObject
{
    #region Variables
    public static Action onDestroy;
    private ParticleSystem hitParticle;
    private AsteroidsSpawner spawner;

    public LayerMask enemyLayer;
    public int damage;
    private bool isDestroyed = false;
    [SerializeField] private GameObject model;

    // Movement
    private MovementController mController;
    private Vector2 direction;

    // Pooled object
    public string _tag;
    public string Tag
    {
        get { return _tag; }
        set { _tag = value; }
    }
    #endregion

    private void Awake()
    {
        mController = GetComponent<MovementController>();
        hitParticle = GetComponentInChildren<ParticleSystem>();
        spawner = AsteroidsSpawner.Instance;
        direction = new Vector2(0f, 1f);
    }

    private void FixedUpdate()
    {
        if (isDestroyed) return;

        mController.MovementUpdate(direction);
    }

    public void OnObjectSpawn()
    {
        model.SetActive(true);
        isDestroyed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(damage);
            hitParticle.transform.position = collision.contacts[0].point;
            StartCoroutine(DestroyOnTime());
        }
    }

    public void OnProjectileDestroy()
    {
        switch (Tag)
        {
            case "bigAsteroid":
                spawner.SpawnAsteroidOnDestroy("mediumAsteroid", this.transform);
                spawner.SpawnAsteroidOnDestroy("mediumAsteroid", this.transform);
                spawner.SpawnAsteroidOnDestroy("mediumAsteroid", this.transform);
                break;
            case "mediumAsteroid":
                spawner.SpawnAsteroidOnDestroy("smallAsteroid", this.transform);
                spawner.SpawnAsteroidOnDestroy("smallAsteroid", this.transform);
                break;
            default:
                break;
        }
        StartCoroutine(DestroyOnTime());
    }

    // Turning off this object
    // Play particle effect end then return object to pool
    public IEnumerator DestroyOnTime()
    {
        model.SetActive(false);
        isDestroyed = true;
        hitParticle.Play();
        onDestroy?.Invoke();

        yield return new WaitForSeconds(1.0f);
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
    }


}
