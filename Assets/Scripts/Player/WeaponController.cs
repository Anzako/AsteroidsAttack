using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Weapons
    private weaponTag actualWeapon = weaponTag.basicWeapon;

    [SerializeField] private GameObject laser;

    private float bonusWeaponLifeTime = 10f;
    private float bonusWeaponElapsedTime = 0f;
    private bool bonusWeaponEquiped = false;

    // Shooting
    [SerializeField] private Transform projectileSpawnPoint;

    private float lastShootTime = 0;
    public float timeToShoot = 0.5f;

    // Animation
    private Animator pAnimator;

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
        ObjectPooler.Instance.SpawnObject(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);
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
    }

    public void ChangeWeapon(weaponTag weaponTag)
    {
        actualWeapon = weaponTag;
        bonusWeaponElapsedTime = 0f;
        bonusWeaponEquiped = true;
    }

}

public enum weaponTag
{
    basicWeapon,
    rocket,
    laser,
}