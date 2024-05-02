using System;
using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
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
        if ((enemyLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            HealthController hController = collision.gameObject.GetComponentInParent<HealthController>();
            hController.TakeDamage(damage);
            hitParticle.transform.position = collision.contacts[0].point;

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
        hitParticle.Play();

        yield return new WaitForSeconds(1.0f);
        AsteroidsSpawner.Instance.ReturnToPool(this.gameObject);
        onDestroy?.Invoke();
    }


}
