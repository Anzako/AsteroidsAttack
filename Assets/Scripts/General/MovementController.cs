using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

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
        RotateHorizontal(horizontalRotationAngle);
    }

    public void MovementFixedUpdate(Vector2 moveDirection)
    {
        RotateToGround();
        Move(moveDirection);
    }

    private void Move(Vector2 moveDirection)
    {
        if (isGrounded)
        {
            Vector3 horizontalVector = -Vector3.Cross(transform.forward, transform.up) * moveDirection.x;
            Vector3 verticalVector = transform.forward * moveDirection.y;
            Vector3 projectedVector = horizontalVector + verticalVector;
            projectedVector.Normalize();

            transform.position += projectedVector * moveSpeed * Time.deltaTime;
        }
    }

    private void RotateHorizontal(float rotationAngle)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(rotationAngle, transform.up);
        transform.rotation = targetRotation * transform.rotation;
    }

    private void RotateToGround()    
    {
        isGrounded = false;

        if (IsInsideMetaballs())
        {
            PushObjectFromGround();
        }

        DrawForceVector();

        if (Physics.Raycast(transform.position, -transform.up, out groundHit, layerMask))
        {
            isGrounded = true;
            //Debug.DrawRay(transform.position, -transform.up, Color.green);  // Visualize ground normal
            float angle = Vector3.Angle(transform.up, groundHit.normal);

            if (angle > maxAngle)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundHit.normal)
                * transform.rotation;

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                    Time.deltaTime * rotationSpeed);
            }
     
            AddGravity();
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

    private void DrawForceVector()
    {
        Vector3 vector = MetaBalls.instance.CalculateMetaballsNormal(transform.position);

        Debug.DrawRay(transform.position, vector, Color.green);
    }

}
