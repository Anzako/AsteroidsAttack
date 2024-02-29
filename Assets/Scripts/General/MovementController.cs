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
    public bool isMoving = true;
    private float angle = 0f;

    public float moveSpeed;
    public float toGroundDistance;
    public float interpolationSpeed;
    public float rotationSpeed;

    // Add checking when player raycast not hitting any surface
    // - check if character is in the surface
    // - detect closest surface and rotate character to it

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
        if (isGrounded && isMoving)
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

        Vector3 axis = Vector3.Cross(-transform.up, transform.forward);
        Quaternion rot = Quaternion.AngleAxis(angle, axis);

        Vector3 tiltedVector = rot * -transform.up;

        if (Physics.Raycast(transform.position, tiltedVector, out groundHit, layerMask) && isMoving)
        {
            isGrounded = true;
            Debug.DrawRay(transform.position, -transform.up, Color.green);  // Visualize ground normal
            float angle = Vector3.Angle(transform.up, groundHit.normal);

            if (transform.up != groundHit.normal)
            {
                if (angle > 5)
                {
                    Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundHit.normal)
                    * transform.rotation;

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                        Time.deltaTime * rotationSpeed);
                }
                
            }

            AddGravity();
        }
    }

    private void AddGravity() 
    {
        float distance = groundHit.distance - toGroundDistance;

        if (Math.Abs(distance) > 0.01)
        {
            Vector3 adjustVector = transform.up * distance;
                
            Vector3 targetPosition = transform.position - adjustVector;
            transform.position = targetPosition;
            //transform.position = Vector3.Lerp(transform.position, targetPosition, interpolationSpeed * Time.deltaTime);
        }
            
        
    }

    public RaycastHit GetGroundHit()
    {
        return groundHit;
    }


}
