using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private PlayerController playerController;

    public float mousePos;
    public float sensitivity;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.GetAxis("Mouse X") * sensitivity;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerController.ShootProjectile();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerController.SetDirection(context.ReadValue<Vector2>());
        } else if (context.canceled)
        {
            playerController.SetDirection(context.ReadValue<Vector2>());
        }
    }

    public void OnSpaceClicked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerController.Dash();
        }
    }
}
