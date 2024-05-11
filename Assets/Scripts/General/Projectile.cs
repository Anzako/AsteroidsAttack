using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    private MovementController mController;
    private ParticleSystem hitParticle;
    [SerializeField] private GameObject model;

    public LayerMask enemyLayer;
    public string objectTag;
    public float timeToDestroy;
    public int damage;

    public bool selfDestroyable = true;
    private bool isDestroyed = false;

    private Vector2 moveDirection;

    public string _tag;
    public string Tag
    {
        get { return _tag; }
        set { _tag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<MovementController>();
        hitParticle = GetComponentInChildren<ParticleSystem>();

        // Setting forward direction
        moveDirection = new Vector2(0, 1); 
    }

    private void OnEnable()
    {
        if (selfDestroyable)
        {
            StartCoroutine(DestroyOnSpawn());
        }
    }

    private void FixedUpdate()
    {
        if (isDestroyed) return;

        mController.MovementUpdate(moveDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(damage);
            hitParticle.transform.position = collision.contacts[0].point;
            StartCoroutine(DestroyOnHit());
        }
    }

    IEnumerator DestroyOnSpawn()
    {
        yield return new WaitForSeconds(timeToDestroy);
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
    }

    IEnumerator DestroyOnHit()
    {
        model.SetActive(false);
        isDestroyed = true;
        Light lightComponent = GetComponent<Light>();
        if (lightComponent != null) lightComponent.enabled = false;
        hitParticle.Play();

        yield return new WaitForSeconds(1.0f);
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
    }

    public void OnObjectSpawn()
    {
        model.SetActive(true);
        isDestroyed = false;
        Light lightComponent = GetComponent<Light>();
        if (lightComponent != null) lightComponent.enabled = true;
    }

}
