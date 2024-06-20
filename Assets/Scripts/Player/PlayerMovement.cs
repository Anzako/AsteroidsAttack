using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MovementController
{
    [SerializeField] private InputController inputController;
    [SerializeField] private PlayerHealth healthController;
    [SerializeField] private ParticleSystem dashParticle;

    private float initialMoveSpeed;

    private bool isFreezed;

    //Dash
    private bool forwardDashUnlocked;
    private bool canDash;
    private bool isDashing;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;

    // Smoothing movement
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float smoothInputSpeed = .2f;

    // Sounds
    [SerializeField] private AudioClip dashSoundClip;

    private void Awake()
    {
        initialMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        canDash = true;
        isDashing = false;
        movementDirection = Vector2.zero;
    }

    protected override void Update()
    {
        if (isFreezed) return;

        base.Update();
        RotateAroundVerticalAxis(inputController.mousePos);
    }

    protected override void Move()
    {
        currentInputVector = Vector2.SmoothDamp(currentInputVector, movementDirection, ref smoothInputVelocity, smoothInputSpeed);
        
        if (isDashing)
        {
            projectedDirection = transform.forward * movementDirection.y + transform.right * movementDirection.x;
        } 
        else
        {
            projectedDirection = transform.forward * currentInputVector.y + transform.right * currentInputVector.x;
        }

        transform.position += actualSpeed * Time.deltaTime * projectedDirection;
    }

    public void SetMovementDirection(Vector2 moveDirection)
    {
        if (!isDashing)
        {
            movementDirection = moveDirection;
        }
    }

    public void DashPressed()
    {
        if (canDash && forwardDashUnlocked)
        {
            if (movementDirection != Vector2.zero && movementDirection.normalized.y == 1)
            {
                StartCoroutine(Dash());
            }
           
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        actualSpeed = dashSpeed;
        SoundFXManager.Instance.PlaySoundFXClip(dashSoundClip, transform, 1f);
        healthController.immortal = true;
        dashParticle.Play();

        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        healthController.immortal = false; 
        ResetActualSpeed();

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void Freeze(bool isFreeze)
    {
        isFreezed = isFreeze;
    }

    public void ResetStats()
    {
        moveSpeed = initialMoveSpeed;
        ResetActualSpeed();
        currentInputVector = Vector2.zero;
        forwardDashUnlocked = false;
    }

    public void IncreaseMoveSpeed(float speed)
    {
        moveSpeed += speed;
        actualSpeed = moveSpeed;
    }

    public void UnlockForwardDash()
    {
        forwardDashUnlocked = true;
    }
}
