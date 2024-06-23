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
    [SerializeField] private GameObject upgradedLaser;
    [SerializeField] private GameObject dashProjectile;
    [SerializeField] private UIController playerHUD;
    private bool laserUpgraded;
    private bool rocketUpgraded;
    [SerializeField] private Transform projectileSpawnGameObject;

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
        UpdateWeaponsSlider();
        if (bonusWeaponElapsedTime >= bonusWeaponLifeTime)
        {
            playerHUD.DisableWeaponSliders();
            bonusWeaponEquiped = false;
            actualWeapon = weaponTag.basicWeapon;
        }
    }

    public void UpdateWeaponsSlider()
    {
        if (actualWeapon == weaponTag.laser)
        {
            playerHUD.SetLaser(bonusWeaponLifeTime - bonusWeaponElapsedTime, bonusWeaponLifeTime);
        } 
        else if (actualWeapon == weaponTag.rocket)
        {
            playerHUD.SetRocket(bonusWeaponLifeTime - bonusWeaponElapsedTime, bonusWeaponLifeTime);
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
        if (!laserUpgraded)
        {
            Instantiate(laser, projectileSpawnPoint);
        } else
        {
            Instantiate(upgradedLaser, projectileSpawnPoint);
        }
    }

    private void ShootRocket()
    {
        Rocket rocket = ObjectPooler.Instance.SpawnObject(poolTags.rocket, projectileSpawnPoint.position, 
            transform.rotation).GetComponent<Rocket>();

        if (!rocketUpgraded)
        {
            rocket.hitDamageAmount = 2;
            rocket.explosionDamageAmount = 2;
            rocket.explosionRadius = 2;
        } else
        {
            rocket.hitDamageAmount = 4;
            rocket.explosionDamageAmount = 4;
            rocket.explosionRadius = 3;
        }
    }

    public void ShootDashProjectile()
    {
        Instantiate(dashProjectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation, projectileSpawnGameObject);
    }

    public void ResetStats()
    {
        bonusWeaponEquiped = false;
        bonusWeaponElapsedTime = 0f;
        actualWeapon = weaponTag.basicWeapon;
        basicWeaponLevel = 1;
        timeToShoot = initialTimeToShoot;
        laserUpgraded = false;
        rocketUpgraded = false;
    }

    public void ChangeWeapon(weaponTag weaponTag)
    {
        actualWeapon = weaponTag;
        if (weaponTag == weaponTag.laser)
        {
            playerHUD.EnableLaserSlider();
        } else if (weaponTag == weaponTag.rocket) 
        { 
            playerHUD.EnableRocketSlider(); 
        }

        bonusWeaponElapsedTime = 0f;
        bonusWeaponEquiped = true;
    }

    public void UpgradeBasicWeapon()
    {
        basicWeaponLevel += 1;
    }

    public void UpgradeLaser()
    {
        laserUpgraded = true;
    }

    public void UpgradeRocket()
    {
        rocketUpgraded = true;
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