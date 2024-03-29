using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private MovementController mController;
    private ParticleSystem onHitParticle;
    [SerializeField] private GameObject model;

    public LayerMask enemyLayer;
    public float timeToDestroy;
    public int damage;

    public bool selfDestroyable = true;
    private bool isDestroyed = false;

    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<MovementController>();
        onHitParticle = GetComponentInChildren<ParticleSystem>();

        // Setting forward direction
        moveDirection = new Vector2(0, 1);

        if (selfDestroyable)
        {
            Destroy(this.gameObject, timeToDestroy);
        }
    }

    private void FixedUpdate()
    {
        if (isDestroyed) return;

        mController.MovementFixedUpdate(moveDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((enemyLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            HealthController hController = collision.gameObject.GetComponent<HealthController>();
            hController.TakeDamage(damage);
            onHitParticle.transform.position = collision.contacts[0].point;

            StartCoroutine(DestroyOnHit());
        }
    }

    IEnumerator DestroyOnHit()
    {
        model.SetActive(false);
        isDestroyed = true;
        Light lightComponent = GetComponent<Light>();
        if (lightComponent != null) lightComponent.enabled = false;
        onHitParticle.Play();

        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }

}
