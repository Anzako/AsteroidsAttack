using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour, IPooledObject
{
    [SerializeField] private poolTags _tag;
    public poolTags Tag 
    { 
        get { return _tag; } 
    }

    // Sounds
    [SerializeField] private AudioClip pickupSoundClip;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.AddShield();
            SoundFXManager.Instance.PlaySoundFXClip(pickupSoundClip, transform, 1f);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }
    }

    public void OnObjectSpawn()
    {
        
    }

  
}
