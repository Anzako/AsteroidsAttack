using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movementController;
    [SerializeField] private PlayerHealth healthController;
    [SerializeField] private UIController HUDController;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject rocket;

    // Shooting
    private float lastShootTime = 0;
    public float timeToShoot;

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;
    }

    public void EnablePlayer()
    {
        gameObject.SetActive(true);
        HUDController.SetActive(true);

        healthController.SetHealthToMax();
    }

    public void DisablePlayer()
    {
        gameObject.SetActive(false);
        HUDController.SetActive(false);
        movementController.ResetActualSpeed();
    }

    public void Freeze(bool isFreeze)
    {
        movementController.Freeze(isFreeze);
    }

    public void ShootProjectile()
    {
        if (lastShootTime >= timeToShoot)
        {
            ObjectPooler.Instance.SpawnObject(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);
            lastShootTime = 0f;
        }
    }

    public void ShootLaser()
    {
        if (lastShootTime >= timeToShoot)
        {
            //Instantiate(laser, projectileSpawnPoint.position, transform.rotation);
            Instantiate(rocket, projectileSpawnPoint.position, transform.rotation);
            lastShootTime = 0f;
        }
    }

}
