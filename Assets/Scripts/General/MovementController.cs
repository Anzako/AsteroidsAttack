using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    private int layerMask;

    private RaycastHit groundHit;
    private bool isGrounded;
    private float maxAngle = 5f;

    public float moveSpeed;
    public float toGroundDistance;
    public float interpolationSpeed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << 7;
    }

    // Update is called once per frame
    public void MovementUpdate(float horizontalRotationAngle)
    {
        RotateAroundVerticalAxis(horizontalRotationAngle);
    }

    public void MovementFixedUpdate(Vector2 moveDirection)
    {
        //Debug.DrawRay(transform.position, -transform.up, Color.green);
        //Debug.DrawRay(transform.position, -transform.forward, Color.red);
        RotateToGround();
        Move(moveDirection.normalized);
    }

    private void Move(Vector2 moveDirection)
    {
        if (!isGrounded) return;
        
        /*Vector3 horizontalVector = -Vector3.Cross(transform.forward, transform.up) * moveDirection.x;
        Vector3 verticalVector = transform.forward * moveDirection.y;
        Vector3 projectedVector = horizontalVector + verticalVector;
        projectedVector.Normalize();

        transform.position += projectedVector * moveSpeed * Time.deltaTime;*/

        Vector3 horizontalVector = Vector3.ProjectOnPlane(transform.forward, transform.up) * moveDirection.y;
        Vector3 projectedVector = horizontalVector + transform.right * moveDirection.x;

        // Unikaj wielokrotnego odwo³ywania siê do transform.position
        Vector3 newPosition = transform.position + projectedVector.normalized * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    private void RotateAroundVerticalAxis(float rotationAngle)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(rotationAngle, transform.up);
        transform.rotation = targetRotation * transform.rotation;
    }

    private void RotateToGround()    
    {
        if (Physics.Raycast(transform.position, -transform.up, out groundHit, layerMask))
        {
            isGrounded = true;
            //Debug.DrawRay(transform.position, -transform.up, Color.green);  // Visualize ground normal

            if (Vector3.Angle(transform.up, groundHit.normal) > maxAngle)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundHit.normal)
                * transform.rotation;

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                    Time.deltaTime * rotationSpeed);
            }
     
            AddGravity();
        } else
        {
            isGrounded = false;
        }

        if (IsInsideMetaballs())
        {
            PushObjectFromGround();
        }
    }

    private void AddGravity() 
    {
        float distance = groundHit.distance - toGroundDistance;

        if (Math.Abs(distance) > 0.01)
        {
            transform.position = transform.position - transform.up * distance;
        }
    }

    private bool IsInsideMetaballs()
    {
        // change 0.5f on treshold variable

        return MetaBalls.instance.CalculateScalarFieldValue(transform.position) > 0.5f;
    }

    private void PushObjectFromGround()
    {
        Vector3 vector = MetaBalls.instance.CalculateMetaballsNormal(transform.position);
        
        transform.position += vector;
        transform.up = vector.normalized;

        //Debug.DrawRay(transform.position, vector, Color.green);
    }

}


