using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Projectile : MonoBehaviour
{
    private MovementController mController;
    private ParticleSystem onHitParticle;
    [SerializeField] private GameObject model;

    public LayerMask enemyLayer;
    public float timeToDestroy = 5f;
    public int damage = 2;

    private bool isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<MovementController>();
        onHitParticle = GetComponentInChildren<ParticleSystem>();
        Destroy(this.gameObject, timeToDestroy);
    }

    private void FixedUpdate()
    {
        Vector2 forwardVector = new Vector2(0, 1);
        if (!isDestroyed)
        {
            mController.MovementFixedUpdate(forwardVector);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((enemyLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            HealthController hController = collision.gameObject.GetComponent<HealthController>();
            hController.TakeDamage(damage);

            StartCoroutine(DestroyOnHit());
        }
    }

    IEnumerator DestroyOnHit()
    {
        model.SetActive(false);
        isDestroyed = true;
        GetComponent<Light>().enabled = false;
        onHitParticle.Play();

        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }

}
