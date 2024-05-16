using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement mController;
    private InputController inputController;
    private PlayerHealth healthController;
    private UIController HUDController;
    public int actualScore = 0;

    Vector2 direction = Vector2.zero;

    // Shooting
    public string projectileTag;
    private float lastShootTime = 0;
    public float timeToShoot;

    private void Awake()
    {
        mController = GetComponent<PlayerMovement>();
        inputController = GetComponent<InputController>();
        healthController = GetComponent<PlayerHealth>();
        HUDController = GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        mController.MovementUpdate(direction);
        mController.MouseUpdate(inputController.mousePos);
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
        mController.ResetActualSpeed();
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

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void Dash()
    {
        if (mController.canDash)
        {
            StartCoroutine(mController.Dash());
        }
    }

}
