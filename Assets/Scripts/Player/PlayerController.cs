using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementController mController;
    [SerializeField] private InputController inputController;
    [SerializeField] private UIController UIController;
    public int actualScore = 0;

    Vector2 direction = Vector2.zero;
    public float mouseSensitivity = 1f;

    // Shooting
    [SerializeField] private GameObject projectile;
    private float lastShootTime = 0;
    public float timeToShoot;

    // Update is called once per frame
    void Update()
    {
        mController.MovementFixedUpdate(direction);
        mController.MovementUpdate(inputController.mousePos.x * mouseSensitivity);
        lastShootTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (mController != null)
        {
            //mController.MovementFixedUpdate(direction);
        }
        
    }

    public void ShootProjectile()
    {
        if (lastShootTime >= timeToShoot)
        {
            Vector3 spawnPosition = transform.position + transform.forward.normalized;
            Instantiate(projectile, spawnPosition, transform.rotation);
            lastShootTime = 0f;
        }
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void AddScore(int score)
    {
        actualScore += score;
        UIController.SetScore(actualScore);
    }

    public void ResetScore()
    {
        actualScore = 0;
        UIController.SetScore(actualScore);
    }
}
