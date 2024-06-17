using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Weapons
    private weaponTag actualWeapon;
    [SerializeField] private GameObject laser;

    private int basicWeaponLevel = 1;

    private float bonusWeaponLifeTime = 10f;
    private float bonusWeaponElapsedTime = 0f;
    private bool bonusWeaponEquiped = false;

    // Shooting
    [SerializeField] private Transform projectileSpawnPoint;

    private float lastShootTime = 0;
    private float initialTimeToShoot;
    [SerializeField] private float timeToShoot = 0.1f;

    // Animation
    private Animator pAnimator;

    private void Awake()
    {
        initialTimeToShoot = timeToShoot;
    }

    private void Start()
    {
        pAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        lastShootTime += Time.deltaTime;

        if (!bonusWeaponEquiped) return;

        bonusWeaponElapsedTime += Time.deltaTime;
        if (bonusWeaponElapsedTime >= bonusWeaponLifeTime)
        {
            bonusWeaponEquiped = false;
            actualWeapon = weaponTag.basicWeapon;
        }
    }

    public void Shoot()
    {
        if (lastShootTime < timeToShoot) return;
        
        switch (actualWeapon)
        {
            case weaponTag.basicWeapon:
                ShootBasicWeapon();
                break;
            case weaponTag.laser:
                ShootLaser();
                break;
            case weaponTag.rocket:
                ShootRocket();
                break;
        }    

        if (pAnimator != null)
        {
            pAnimator.SetTrigger("TrShoot");
        }

        lastShootTime = 0f;
        
    }

    private void ShootBasicWeapon()
    {
        if (basicWeaponLevel == 1) 
        {
            Spawner.SpawnPoolObjectOnPosition(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);
        } else if (basicWeaponLevel == 2)
        {
            float angle = 4;
            GameObject projectile1 = Spawner.SpawnPoolObjectOnPosition(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);
            projectile1.transform.RotateAround(projectile1.transform.position, projectile1.transform.up, angle);
            GameObject projectile2 = Spawner.SpawnPoolObjectOnPosition(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);
            projectile2.transform.RotateAround(projectile2.transform.position, projectile2.transform.up, -angle);
        } else if (basicWeaponLevel == 3)
        {
            Spawner.SpawnPoolObjectOnPosition(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);

            float angle = 5;
            GameObject projectile1 = Spawner.SpawnPoolObjectOnPosition(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);
            projectile1.transform.RotateAround(projectile1.transform.position, projectile1.transform.up, angle);
            GameObject projectile2 = Spawner.SpawnPoolObjectOnPosition(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);
            projectile2.transform.RotateAround(projectile2.transform.position, projectile2.transform.up, -angle);
        } else
        {
            Spawner.SpawnPoolObjectOnPosition(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);
            Debug.Log("Basic weapon error");
        }
                
    }

    private void ShootLaser()
    {
        Instantiate(laser, projectileSpawnPoint);
    }

    private void ShootRocket()
    {
        ObjectPooler.Instance.SpawnObject(poolTags.rocket, projectileSpawnPoint.position, transform.rotation);
    }

    public void ResetStats()
    {
        bonusWeaponEquiped = false;
        bonusWeaponElapsedTime = 0f;
        actualWeapon = weaponTag.basicWeapon;
        basicWeaponLevel = 1;
        timeToShoot = initialTimeToShoot;
    }

    public void ChangeWeapon(weaponTag weaponTag)
    {
        actualWeapon = weaponTag;
        bonusWeaponElapsedTime = 0f;
        bonusWeaponEquiped = true;
    }

    public void UpgradeBasicWeapon()
    {
        basicWeaponLevel += 1;
    }

    public void IncreaseShootingSpeed(float time)
    {
        timeToShoot -= time;
    }

}

public enum weaponTag
{
    basicWeapon,
    rocket,
    laser,
}