using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour, IPooledObject
{
    public float timeToDestroy;
    public int damageAmount;

    [SerializeField] private poolTags particleTag;

    // Sounds
    [SerializeField] private AudioClip hitSoundClip;
    [SerializeField] private AudioClip projectileShootSoundClip;

    public poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
        set { _tag = value; }
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyOnSpawn());
        if (projectileShootSoundClip != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(projectileShootSoundClip, transform, 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(damageAmount);
            if (hitSoundClip != null)
            {
                SoundFXManager.Instance.PlaySoundFXClip(hitSoundClip, transform, 1f);
            }

            ObjectPooler.Instance.SpawnObject(particleTag, transform.position, transform.rotation);
            ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
        }
    }

    IEnumerator DestroyOnSpawn()
    {
        yield return new WaitForSeconds(timeToDestroy);
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
    }

    public void OnObjectSpawn()
    {
        TrailRenderer trailRenderer = GetComponentInChildren<TrailRenderer>();

        if (trailRenderer != null) trailRenderer.Clear();
    }

}
