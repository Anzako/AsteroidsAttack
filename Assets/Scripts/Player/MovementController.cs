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
        rotateToAngle();
    }

    private void FixedUpdate()
    {
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

    private void rotateToAngle()
    {
        if (Physics.Raycast(transform.position, -transform.up, out groundHit))
        {
            Vector3 up = groundHit.normal;

            // Obliczanie rotacji miedzy aktualna normalna gracza a groundHit.normal
            // Dodanie tej rotacji do aktualnej rotacji gracza
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
            transform.rotation = targetRotation;
        }
    }

    

}
