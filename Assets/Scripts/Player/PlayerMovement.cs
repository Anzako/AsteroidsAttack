using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MovementController
{
    [SerializeField] private InputController inputController;
    [SerializeField] private PlayerHealth healthController;
    [SerializeField] private ParticleSystem dashParticle;
    [SerializeField] private WeaponController weaponController;

    private bool isFreezed;

    //Dash
    private bool forwardDashUnlocked;
    private bool backwardDashUnlocked;
    private bool canDash;
    private bool isDashing;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;

    // Smoothing movement
    private float initialMoveSpeed;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float smoothInputSpeed = .2f;

    // Sounds
    [SerializeField] private AudioClip dashSoundClip;

    private void Awake()
    {
        initialMoveSpeed = moveSpeed;
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
        if (canDash)
        {
            if (movementDirection.normalized.y == 1 && forwardDashUnlocked)
            {
                StartCoroutine(ForwardDash());
            }
            else if (movementDirection.normalized.y == -1 && backwardDashUnlocked)
            {
                StartCoroutine(BackwardDash());
            }
        }
    }

    private IEnumerator ForwardDash()
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

    private IEnumerator BackwardDash()
    {
        canDash = false;
        isDashing = true;
        actualSpeed = dashSpeed;
        SoundFXManager.Instance.PlaySoundFXClip(dashSoundClip, transform, 1f);
        healthController.immortal = true;
        weaponController.ShootDashProjectile();
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
        canDash = true;
        isDashing = false;
        movementDirection = Vector2.zero;
        currentInputVector = Vector2.zero;
        moveSpeed = initialMoveSpeed;
        ResetActualSpeed();
        forwardDashUnlocked = false;
        backwardDashUnlocked = false;
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

    public void UnlockBackwardDash()
    {
        backwardDashUnlocked = true;
    }
}
