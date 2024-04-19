using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private MetaBalls metaballs;
    public float moveSpeed;

    public static float toGroundPotential = 0.47f;
    public static float gravityForce = 4f;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        metaballs = MetaBalls.instance;
    }

    // Update is called once per frame
    public void PlayerMouseUpdate(float horizontalRotationAngle)
    {
        RotateAroundVerticalAxis(horizontalRotationAngle);
    }

    public void MovementUpdate(Vector2 moveDirection)
    {
        RotateToSurface();
        Move(moveDirection.normalized);
    }

    private void Move(Vector2 moveDirection)
    {
        Vector3 projectedVector = transform.forward * moveDirection.y + transform.right * moveDirection.x;

        transform.position += projectedVector.normalized * moveSpeed * Time.deltaTime;
    }

    private void RotateAroundVerticalAxis(float rotationAngle)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(rotationAngle, transform.up);
        transform.rotation = targetRotation * transform.rotation;
    }

    private void RotateToSurface()    
    {
        Vector3 potentialVector = metaballs.CalculateMetaballsNormal(transform.position);
        Debug.DrawRay(transform.position, potentialVector.normalized, Color.red);

        float scalarValue = metaballs.CalculateScalarFieldValue(transform.position);

        // Rotating object to new rotation depending on potential vector
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, potentialVector.normalized)
                * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            Time.deltaTime * rotationSpeed);

        float val = toGroundPotential - scalarValue;
        if (Math.Abs(val) > 0.01f)
        {
            transform.position -= potentialVector.normalized * val * gravityForce;
        }
    }

}
