using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour 
{
    public float timeToDestroy;
    public int hitDamageAmount;
    public int explosionDamageAmount;
    public float explosionRadius;

    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private ParticleSystem exposionParticles;
    [SerializeField] private poolTags particleTag;

    private void OnEnable()
    {
        StartCoroutine(DestroyOnSpawn());
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(hitDamageAmount);
            Destroy(collision.contacts[0].point);
        }
    }

    IEnumerator DestroyOnSpawn()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(transform.position);
    }

    private void Destroy(Vector3 position)
    {
        ObjectPooler.Instance.SpawnObject(particleTag, position, transform.rotation);
        Explode(position);
        Destroy(this.gameObject);
    }

    private void Explode(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius, enemyLayers);

        foreach (Collider hit in hits)
        {
            IDamagable damagable = hit.gameObject.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                damagable.Damage(explosionDamageAmount);
            }
            
        }
    }

}
