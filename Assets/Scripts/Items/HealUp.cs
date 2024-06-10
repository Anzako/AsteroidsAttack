using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUp : MonoBehaviour, IPooledObject
{
    public int healAmount;

    // Sounds
    [SerializeField] private AudioClip healUpSoundClip;

    [SerializeField] private poolTags _tag;
    public poolTags Tag { get { return _tag; } }

    public void OnObjectSpawn()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        IHealable healable = collision.gameObject.GetComponentInParent<IHealable>();

        if (healable != null )
        {
            healable.Heal(healAmount);
            SoundFXManager.Instance.PlaySoundFXClip(healUpSoundClip, transform, 1f);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }
    }
}
