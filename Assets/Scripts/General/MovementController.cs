using UnityEngine;

public class MovementController : MonoBehaviour
{
    private MetaBalls metaballs;
    public float moveSpeed;
    protected float actualSpeed;

    protected Vector2 movementDirection;
    protected Vector3 projectedDirection;

    // Gravity variables
    public static float toGroundPotential = 0.47f;
    public static float gravityForce = 4f;
    public float rotationSpeed;

    void Start()
    {
        metaballs = MetaBalls.Instance;
        movementDirection = Vector2.zero;
        projectedDirection = Vector3.zero;
        actualSpeed = moveSpeed;
    }

    public void MovementUpdate(Vector2 moveDirection)
    {
        SetMovementDirection(moveDirection);
        RotateToSurface();
        Move();
    }

    private void Move()
    {
        projectedDirection = transform.forward * movementDirection.y + transform.right * movementDirection.x;
        projectedDirection.Normalize();

        transform.position += actualSpeed * Time.deltaTime * projectedDirection;
    }

    private void RotateToSurface()    
    {
        Vector3 potentialVector = metaballs.CalculateMetaballsNormal(transform.position);
        Debug.DrawRay(transform.position, potentialVector.normalized, Color.red);

        // Rotating object to new rotation depending on potential vector
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, potentialVector.normalized)
                * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            Time.deltaTime * rotationSpeed);

        float val = toGroundPotential - metaballs.CalculateScalarFieldValue(transform.position);
        transform.position -= potentialVector.normalized * val * gravityForce;
    }

    protected void RotateAroundVerticalAxis(float rotationAngle)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(rotationAngle, transform.up);
        transform.rotation = targetRotation * transform.rotation;
    }

    public void ResetActualSpeed()
    {
        actualSpeed = moveSpeed;
    }

    protected virtual void SetMovementDirection(Vector2 moveDirection)
    {
        movementDirection = moveDirection.normalized;
    }
}
