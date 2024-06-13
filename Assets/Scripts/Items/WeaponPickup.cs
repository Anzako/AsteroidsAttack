using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IPooledObject
{
    [SerializeField] private poolTags _tag;
    public poolTags Tag { get { return _tag; } }

    [SerializeField] private weaponTag weaponTag;
    [SerializeField] private float weaponTime;

    private void OnCollisionEnter(Collision collision)
    {
        WeaponController playerWeapon = collision.gameObject.GetComponentInParent<WeaponController>();

        if (playerWeapon != null )
        {
            playerWeapon.ChangeWeapon(weaponTag);
            ObjectPooler.Instance.ReturnObjectToPool(gameObject);
        }
    }

    public void OnObjectSpawn()
    {

    }
}
