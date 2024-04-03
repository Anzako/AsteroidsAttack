using System.Collections;
using UnityEngine;

public class AsteroidController : MonoBehaviour, IPooledObject
{
    // Projectile variables
    private ParticleSystem onHitParticle;
    public LayerMask enemyLayer;
    public int damage;
    private bool isDestroyed = false;
    [SerializeField] private GameObject model;

    // Asteroid variables
    public enum AsteroidSize
    {
        Big,
        Medium,
        Small
    }
    public AsteroidSize _size;

    // Movement
    private MovementController mController;
    public Vector2 direction;

    // Pooled object
    private string _tag;
    public string Tag
    {
        get { return _tag; }
        set { _tag = value; }
    }

    private void Awake()
    {
        mController = GetComponent<MovementController>();
        onHitParticle = GetComponentInChildren<ParticleSystem>();
        Tag = "asteroid";
        _size = AsteroidSize.Big;
        direction = new Vector2(0f, 1f);
    }

    private void FixedUpdate()
    {
        if (isDestroyed) return;

        mController.MovementFixedUpdate(direction);

        Debug.DrawRay(transform.position, -transform.up, Color.green);
        Debug.DrawRay(transform.position, -transform.forward, Color.red);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((enemyLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            HealthController hController = collision.gameObject.GetComponent<HealthController>();
            hController.TakeDamage(damage);
            onHitParticle.transform.position = collision.contacts[0].point;

            OnObjectDestroy();
        }
    }

    public void OnObjectSpawn()
    {
        UpdateAsteroidParameters();
        model.SetActive(true);
        isDestroyed = false;
    }

    public void OnObjectDestroy()
    {
        StartCoroutine(AsteroidDestroy(this._size));
    }

    public IEnumerator AsteroidDestroy(AsteroidSize size)
    {
        model.SetActive(false);
        isDestroyed = true;
        onHitParticle.Play();
        
        switch (size)
        {
            case AsteroidSize.Big:
                AsteroidsSpawner.SpawnAsteroidOnDestroy(AsteroidSize.Medium, this.transform);
                AsteroidsSpawner.SpawnAsteroidOnDestroy(AsteroidSize.Medium, this.transform);
                AsteroidsSpawner.SpawnAsteroidOnDestroy(AsteroidSize.Medium, this.transform);
                break;
            case AsteroidSize.Medium:
                AsteroidsSpawner.SpawnAsteroidOnDestroy(AsteroidSize.Small, this.transform);
                AsteroidsSpawner.SpawnAsteroidOnDestroy(AsteroidSize.Small, this.transform);
                break;
            case AsteroidSize.Small:
                Debug.Log("Asteroid destroyed");
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(1.0f);
        ObjectPooler.instance.ReturnObjectToPool(this.gameObject);
    }

    public void UpdateAsteroidParameters()
    {
        // update size and maybe hp

        switch (_size)
        {
            case AsteroidSize.Big:
                transform.localScale = Vector3.one;
                break;
            case AsteroidSize.Medium:
                transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                break;
            case AsteroidSize.Small:
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
        }
    }
}
