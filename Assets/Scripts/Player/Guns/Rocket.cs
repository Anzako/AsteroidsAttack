using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, IPooledObject
{
    public float timeToDestroy;
    public int hitDamageAmount;
    public int explosionDamageAmount;
    public float explosionRadius;

    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private poolTags particleTag;

    // Sounds
    [SerializeField] private AudioClip hitSoundClip;
    [SerializeField] private AudioClip rocketShootSoundClip;

    // Pooled object
    [SerializeField] private poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyOnSpawn());
        SoundFXManager.Instance.PlaySoundFXClip(rocketShootSoundClip, transform, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(hitDamageAmount);
            SoundFXManager.Instance.PlaySoundFXClip(hitSoundClip, transform, 1f);
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
        // Explosion particles and damage
        ObjectPooler.Instance.SpawnObject(particleTag, position, transform.rotation);
        Explode(position);

        // Returning object to pool
        ObjectPooler.Instance.ReturnObjectToPool(gameObject);
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

    public void OnObjectSpawn()
    {
        
    }
}
