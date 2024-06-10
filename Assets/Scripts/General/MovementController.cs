using Unity.VisualScripting;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float moveSpeed;
    protected float actualSpeed;

    [SerializeField] protected Vector2 movementDirection = new(0, 1);
    protected Vector3 projectedDirection = Vector2.zero;

    // Gravity variables
    protected float toGroundPotential;
    public float aboveGroundDistance = 0;
    public static float gravityForce = 4f;
    public float rotationSpeed;

    protected virtual void Start()
    {
        actualSpeed = moveSpeed;
        toGroundPotential = MarchingCubes.isoLevel;
    }

    protected virtual void Update()
    {
        RotateToSurface();
        Move();
    }

    protected virtual void Move()
    {
        projectedDirection = transform.forward * movementDirection.y + transform.right * movementDirection.x;

        transform.position += actualSpeed * Time.deltaTime * projectedDirection;
    }

    // Slerp rotation
    protected void RotateToSurface()    
    {
        Vector3 potentialVector = MetaBalls.CalculateMetaballsNormal(transform.position);
        Debug.DrawRay(transform.position, potentialVector.normalized, Color.red);

        // Rotating object to new rotation depending on potential vector
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, potentialVector.normalized)
                * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            Time.deltaTime * rotationSpeed);

        float val = toGroundPotential - MetaBalls.CalculateScalarFieldValue(transform.position);
        transform.position -= gravityForce * val * potentialVector.normalized 
            - potentialVector.normalized * aboveGroundDistance;
    }

    #region Static Functions
    // No slerp rotation
    public static void RotateToSurface(Transform transform, float aboveGroundDistance)
    {
        Vector3 potentialVector = MetaBalls.CalculateMetaballsNormal(transform.position);
        Debug.DrawRay(transform.position, potentialVector.normalized, Color.red);

        // Rotating object to new rotation depending on potential vector
        transform.rotation = Quaternion.FromToRotation(transform.up, potentialVector.normalized)
                * transform.rotation;
        
        float val = MarchingCubes.isoLevel - MetaBalls.CalculateScalarFieldValue(transform.position);
        transform.position -= gravityForce * val * potentialVector.normalized 
            - potentialVector.normalized * aboveGroundDistance;
    }

    public static void Move(Transform transform, Vector2 movementDirection, float distance)
    {
        Vector3 projectedDirection = transform.forward * movementDirection.y + transform.right * movementDirection.x;

        transform.position += distance * projectedDirection;
    }
    #endregion
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
