using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    public float timeToDestroy;
    public int damageAmount;

    [SerializeField] private poolTags particleTag;

    public poolTags _tag;
    public poolTags Tag
    {
        get { return _tag; }
        set { _tag = value; }
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyOnSpawn());
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponentInParent<IDamagable>();

        if (damagable != null)
        {
            damagable.Damage(damageAmount);
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
        ObjectPooler.Instance.ReturnObjectToPool(this.gameObject);
    }

    public void OnObjectSpawn()
    {
        
    }

}
