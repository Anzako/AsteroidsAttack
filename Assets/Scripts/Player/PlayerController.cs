using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movementController;
    [SerializeField] private PlayerHealth healthController;
    [SerializeField] private UIController HUDController;
    [SerializeField] private WeaponController weaponController;

    public void EnablePlayer()
    {
        gameObject.SetActive(true);
        HUDController.SetActive(true);

        ResetStats();
    }

    public void DisablePlayer()
    {
        gameObject.SetActive(false);
        HUDController.SetActive(false);
    }

    private void ResetStats()
    {
        healthController.ResetStats();
        movementController.ResetStats();
    }

    public void Freeze(bool isFreeze)
    {
        movementController.Freeze(isFreeze);
    }

    public void Shoot()
    {
        weaponController.Shoot();
    }

    public void UpgradeHealth(int value) 
    { 
        healthController.IncreaseHealth(value);
    }

}
