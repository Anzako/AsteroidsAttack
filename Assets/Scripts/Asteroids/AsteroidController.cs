using System.Collections;
using System.Drawing;
using UnityEngine;

public class AsteroidController : MonoBehaviour, IPooledObject
{
    #region Variables
    private AsteroidsHealth healthCtr;
    private ParticleSystem onHitParticle;
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
        healthCtr = GetComponent<AsteroidsHealth>();
        mController = GetComponent<MovementController>();
        onHitParticle = GetComponentInChildren<ParticleSystem>();
        spawner = AsteroidsSpawner.instance;
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
        if ((enemyLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            HealthController hController = collision.gameObject.GetComponentInParent<HealthController>();
            hController.TakeDamage(damage);
            onHitParticle.transform.position = collision.contacts[0].point;

            StartCoroutine(AsteroidDestroy(Tag));
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
        StartCoroutine(AsteroidDestroy(Tag));
    }

    // Turning off this object
    // Play particle effect end then return object to pool
    public IEnumerator AsteroidDestroy(string size)
    {
        model.SetActive(false);
        isDestroyed = true;
        onHitParticle.Play();

        yield return new WaitForSeconds(1.0f);
        AsteroidsSpawner.instance.ReturnToPool(this.gameObject);
    }


}
