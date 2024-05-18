using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera pCamera;
    private PlayerMovement movementController;
    private InputController inputController;
    private PlayerHealth healthController;
    private UIController HUDController;

    private bool isFreezed = false;
    private Vector2 movementDirection = Vector2.zero;

    // Shooting
    private string projectileTag;
    private float lastShootTime = 0;
    public float timeToShoot;

    private void Awake()
    {
        movementController = GetComponent<PlayerMovement>();
        inputController = GetComponent<InputController>();
        healthController = GetComponent<PlayerHealth>();
        HUDController = GetComponent<UIController>();
    }
    private void Start()
    {
        projectileTag = poolTags.playerProjectile.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFreezed) return;
        
        movementController.MovementUpdate(movementDirection);
        movementController.MouseUpdate(inputController.mousePos);
        lastShootTime += Time.deltaTime;
    }

    public void EnablePlayer()
    {
        gameObject.SetActive(true);
        HUDController.SetActive(true);

        healthController.SetHealthToMax();
        movementDirection = Vector2.zero;
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
            Vector3 spawnPosition = transform.position + transform.forward.normalized;
            ObjectPooler.Instance.SpawnObject(projectileTag, spawnPosition, transform.rotation);
            lastShootTime = 0f;
        }
    }

    public void SetMovementDirection(Vector2 direction)
    {
        movementDirection = direction;
    }

    public void Dash()
    {
        if (movementController.canDash)
        {
            StartCoroutine(movementController.Dash());
        }
    }

}
