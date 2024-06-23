using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashProjectile : MonoBehaviour
{
    public float timeToDestroy;
    public int damageAmount;
    private List<IDamagable> damagedObjects;

    [SerializeField] private poolTags particleTag;

    // Sounds
    [SerializeField] private AudioClip hitSoundClip;
    [SerializeField] private AudioClip projectileShootSoundClip;

    private void OnEnable()
    {
        damagedObjects = new List<IDamagable>();
        StartCoroutine(DestroyOnSpawn());
        if (projectileShootSoundClip != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(projectileShootSoundClip, transform, 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null && !damagedObjects.Contains(damagable))
        {
            damagedObjects.Add(damagable);
            damagable.Damage(damageAmount);
            if (hitSoundClip != null)
            {
                SoundFXManager.Instance.PlaySoundFXClip(hitSoundClip, transform, 1f);
            }

            ObjectPooler.Instance.SpawnObject(particleTag, transform.position, transform.rotation);
        }
    }

    IEnumerator DestroyOnSpawn()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }

}
