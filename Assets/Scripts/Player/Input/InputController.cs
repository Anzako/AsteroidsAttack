using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerMovement movementController;

    public float mousePos;
    [SerializeField] private float sensitivity;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        movementController = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.GetAxis("Mouse X") * sensitivity;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.State == GameState.Game)
        {
            if (context.performed)
            {
                playerController.ShootProjectile();
            }
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.State == GameState.Game)
        {
            if (context.performed)
            {
                movementController.SetMovementDirection(context.ReadValue<Vector2>());
            }
            else if (context.canceled)
            {
                movementController.SetMovementDirection(context.ReadValue<Vector2>());
            }
        }
    }

    public void OnSpaceClicked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.Instance.State == GameState.Game)
            {
                movementController.DashPressed();
            }
        }
    }

    public void OnEscapeClicked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.Instance.State == GameState.Game)
            {
                GameManager.Instance.ChangeState(GameState.InGameMenu);
            } else if (GameManager.Instance.State == GameState.InGameMenu)
            {
                GameManager.Instance.ChangeState(GameState.Game);
            }
        }
    }
}
