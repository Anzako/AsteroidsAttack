using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private poolTags projectileTag = poolTags.playerProjectile;
    private poolTags actualWeaponTag = poolTags.playerProjectile;

    private float bonusWeaponLifeTime = 10f;
    private float bonusWeaponElapsedTime = 0f;
    private bool bonusWeaponEquiped = false;

    // Shooting
    [SerializeField] private Transform projectileSpawnPoint;

    private float lastShootTime = 0;
    public float timeToShoot = 0.5f;

    private void Update()
    {
        lastShootTime += Time.deltaTime;

        if (!bonusWeaponEquiped) return;

        bonusWeaponElapsedTime += Time.deltaTime;
        if (bonusWeaponElapsedTime >= bonusWeaponLifeTime)
        {
            bonusWeaponEquiped = false;
            actualWeaponTag = projectileTag;
        }
    }

    public void Shoot()
    {
        if (lastShootTime >= timeToShoot)
        {
            ObjectPooler.Instance.SpawnObject(actualWeaponTag, projectileSpawnPoint.position, transform.rotation);
            lastShootTime = 0f;
        }
    }

    public void ChangeWeapon(poolTags weaponTag)
    {
        actualWeaponTag = weaponTag;
        bonusWeaponElapsedTime = 0f;
        bonusWeaponEquiped = true;
    }

}
