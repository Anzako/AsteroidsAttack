using Unity.VisualScripting;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float moveSpeed;
    protected float actualSpeed;

    [SerializeField] protected Vector2 movementDirection = new(0, 1);
    protected Vector3 projectedDirection = Vector2.zero;

    // Gravity variables
    public static float toGroundPotential = 0.47f;
    public static float gravityForce = 4f;
    public float rotationSpeed;

    protected virtual void Start()
    {
        actualSpeed = moveSpeed;
    }

    protected virtual void Update()
    {
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
        Vector3 potentialVector = MetaBalls.CalculateMetaballsNormal(transform.position);
        Debug.DrawRay(transform.position, potentialVector.normalized, Color.red);

        // Rotating object to new rotation depending on potential vector
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, potentialVector.normalized)
                * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            Time.deltaTime * rotationSpeed);

        float val = toGroundPotential - MetaBalls.CalculateScalarFieldValue(transform.position);
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

}
