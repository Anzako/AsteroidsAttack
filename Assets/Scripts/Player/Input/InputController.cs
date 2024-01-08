using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private PlayerController playerController;

    private Vector2 lastMousePos;
    public Vector2 mousePos;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.ReadValue() - lastMousePos;
        lastMousePos = Mouse.current.position.ReadValue();
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
}
