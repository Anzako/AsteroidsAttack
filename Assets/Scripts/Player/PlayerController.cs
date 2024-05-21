using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movementController;
    private PlayerHealth healthController;
    private UIController HUDController;
    [SerializeField] private Transform projectileSpawnPoint;

    private bool isFreezed = false;

    // Shooting
    private float lastShootTime = 0;
    public float timeToShoot;

    private void Awake()
    {
        movementController = GetComponent<PlayerMovement>();
        healthController = GetComponent<PlayerHealth>();
        HUDController = GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFreezed) return;
        
        movementController.MovementUpdate();
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
        isFreezed = isFreeze;
    }

    public void ShootProjectile()
    {
        if (lastShootTime >= timeToShoot)
        {
            ObjectPooler.Instance.SpawnObject(poolTags.playerProjectile, projectileSpawnPoint.position, transform.rotation);
            lastShootTime = 0f;
        }
    }

}
