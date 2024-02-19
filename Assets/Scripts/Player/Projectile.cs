using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private MovementController mController;
    public LayerMask enemyLayer;
    public float timeToDestroy = 5f;
    public int damage = 2;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

    private void FixedUpdate()
    {
        //Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
        Vector2 forwardVector = new Vector2(0, 1);
        mController.MovementFixedUpdate(forwardVector);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((enemyLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            HealthController hController = collision.gameObject.GetComponent<HealthController>();
            hController.TakeDamage(damage);

            ParticleSystem hitPoint = collision.gameObject.GetComponentInChildren<ParticleSystem>();
            hitPoint.transform.position = collision.GetContact(0).point;
            
            Destroy(this.gameObject);
        }
    }

}
