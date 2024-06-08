using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MovementController
{
    [SerializeField] private InputController inputController;

    private bool isFreezed;
    public bool canDash;
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

    protected override void Start()
    {
        base.Start();
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
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        actualSpeed = dashSpeed;
        SoundFXManager.Instance.PlaySoundFXClip(dashSoundClip, transform, 1f);

        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        ResetActualSpeed();

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void Freeze(bool isFreeze)
    {
        isFreezed = isFreeze;
    }

}
