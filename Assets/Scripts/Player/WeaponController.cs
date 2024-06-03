using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private poolTags projectileTag = poolTags.playerProjectile;
    private poolTags actualWeaponTag = poolTags.laser;

    [SerializeField] private Transform projectileSpawnPoint;

    // Shooting
    private float lastShootTime = 0;
    public float timeToShoot = 0.5f;

    private void Update()
    {
        lastShootTime += Time.deltaTime;
    }

    public void Shoot()
    {
        if (lastShootTime >= timeToShoot)
        {
            ObjectPooler.Instance.SpawnObject(actualWeaponTag, projectileSpawnPoint.position, transform.rotation);
            lastShootTime = 0f;
        }
    }


}
