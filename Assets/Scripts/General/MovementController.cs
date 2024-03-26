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
    private MetaBalls metaballs;

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
        metaballs = MetaBalls.instance;
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

        if (Physics.Raycast(transform.position, -transform.up, out groundHit, layerMask))
        {
            isGrounded = true;
            Debug.DrawRay(transform.position, -transform.up, Color.green);  // Visualize ground normal
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
            Vector3 adjustVector = transform.up * distance;
                
            Vector3 targetPosition = transform.position - adjustVector;
            transform.position = targetPosition;
        }
    }

    private bool IsInsideMetaballs()
    {
        float scalarFieldValue = metaballs.CalculateScalarFieldValue(transform.position);

        return scalarFieldValue > 0.5f;
    }

    private void PushObjectFromGround()
    {
        Vector3 vector = metaballs.CalculateMetaballsNormal(transform.position);
        Debug.DrawRay(transform.position, vector, Color.green);
        transform.position += vector;
        transform.up = vector.normalized;
    }

}
