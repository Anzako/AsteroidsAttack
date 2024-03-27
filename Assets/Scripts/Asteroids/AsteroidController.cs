using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.XR;

public class AsteroidController : MonoBehaviour
{
    // Movement
    private MovementController mController;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<MovementController>();
        direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        mController.MovementFixedUpdate(direction);
    }
}
