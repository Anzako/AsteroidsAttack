using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public InputAction playerControls;

    public Vector2 moveDirection { get; private set; }

    private Vector2 lastMousePos;
    public Vector2 mousePos;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();

        mousePos = Mouse.current.position.ReadValue() - lastMousePos;
        lastMousePos = Mouse.current.position.ReadValue();
    }
}
