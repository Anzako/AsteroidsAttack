using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerMovement movementController;
    private GameManager gameManager;

    private PlayerInput playerInput;
    private InputAction moveAction;

    public float mousePos;
    [SerializeField] private float sensitivity;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        movementController = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Movement"];
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.State != GameState.Game) return;

        mousePos = Input.GetAxis("Mouse X") * sensitivity;
        movementController.SetMovementDirection(moveAction.ReadValue<Vector2>());
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (gameManager.State != GameState.Game) return;
        
        if (context.performed)
        {
            playerController.ShootProjectile();
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (gameManager.State != GameState.Game) return;

        if (context.performed)
        {
            playerController.ShootLaser();
        }
    }

    public void OnSpaceClicked(InputAction.CallbackContext context)
    {
        if (gameManager.State != GameState.Game) return;

        if (context.performed)
        {
            movementController.DashPressed();
        }
    }

    public void OnEscapeClicked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (gameManager.State == GameState.Game)
            {
                gameManager.ChangeState(GameState.InGameMenu);
            } 
            else if (gameManager.State == GameState.InGameMenu)
            {
                gameManager.ChangeState(GameState.Game);
            }
        }
    }
}
