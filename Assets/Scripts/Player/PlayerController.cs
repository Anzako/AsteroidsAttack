using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MovementController mController;
    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
