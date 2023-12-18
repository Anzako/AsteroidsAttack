using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class MovementController : MonoBehaviour
{
    private InputController inputController;
    private Rigidbody rb;
    private RaycastHit groundHit;

    public float moveSpeed;
    public float toGroundDistance;
    public float interpolationSpeed;
    public float rotationSpeed;

    public float sensitivity;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputController = GetComponent<InputController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateToMouse(inputController.mousePos);
    }

    private void FixedUpdate()
    {
        RotateToAngle();
        Move(inputController.moveDirection);
    }

    private void Move(Vector2 moveDirection)
    {
        Vector3 horizontalVector = -Vector3.Cross(transform.forward, transform.up) * moveDirection.x;
        Vector3 verticalVector = transform.forward * moveDirection.y;
        Vector3 projectedVector = horizontalVector + verticalVector;
        projectedVector.Normalize();

        rb.velocity = projectedVector * moveSpeed;
    }

    private void RotateToMouse(Vector2 mousePos)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(mousePos.x * sensitivity, transform.up);
        transform.rotation = targetRotation * transform.rotation;
    }

    private void RotateToAngle()    {
        if (Physics.Raycast(transform.position, -transform.up, out groundHit))
        {
            
            Debug.DrawRay(transform.position, groundHit.normal * 2, Color.green);  // Visualize ground normal
            
            if (transform.up != groundHit.normal)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;

                rb.angularVelocity = Vector3.zero;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            AddGravity();
        }
    }

    private void AddGravity() 
    {
        if (groundHit.distance > toGroundDistance)
        {
            float distance = groundHit.distance - toGroundDistance;
            if (distance > 0.01)
            {
                Vector3 adjustVector = transform.up * distance;
                
                Vector3 targetPosition = transform.position - adjustVector;
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * interpolationSpeed);
            }
            
        }
    }

}
