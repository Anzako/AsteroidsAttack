using System.Collections;
using UnityEngine;

public class PlayerMovement : MovementController
{
    public bool canDash;
    private bool isDashing;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;

    private void OnEnable()
    {
        canDash = true;
        isDashing = false;
    }

    public void MouseUpdate(float horizontalRotationAngle)
    {
        RotateAroundVerticalAxis(horizontalRotationAngle);
    }

    protected override void SetMovementDirection(Vector2 moveDirection)
    {
        if (!isDashing)
        {
            base.SetMovementDirection(moveDirection);
        }
    }

    public IEnumerator Dash()
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

}
