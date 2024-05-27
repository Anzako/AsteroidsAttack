using System.Collections;
using UnityEngine;

public class PlayerMovement : MovementController
{
    [SerializeField] private InputController inputController;

    private bool isFreezed;
    public bool canDash;
    private bool isDashing;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;

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

    public void SetMovementDirection(Vector2 moveDirection)
    {
        if (!isDashing)
        {
            movementDirection = moveDirection.normalized;
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
