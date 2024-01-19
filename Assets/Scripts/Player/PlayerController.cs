using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementController mController;
    [SerializeField] private InputController inputController;
    
    Vector2 direction = Vector2.zero;
    public float mouseSensitivity = 0.5f;

    // Prefabs
    [SerializeField] private GameObject projectile;

    // Update is called once per frame
    void Update()
    {
        mController.MovementUpdate(inputController.mousePos.x * mouseSensitivity);
    }

    private void FixedUpdate()
    {
        mController.MovementFixedUpdate(direction);
    }

    public void ShootProjectile()
    {
        Vector3 spawnPosition = transform.position + transform.forward.normalized;
        Instantiate(projectile, spawnPosition, transform.rotation);
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }
}
