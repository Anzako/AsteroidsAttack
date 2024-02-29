using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLight : MonoBehaviour
{
    private Transform lightTransform;
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        lightTransform = GetComponent<Transform>();
    }

    
    void Update()
    {
        SetDirectionToPlayerCamera();
    }

    private void SetDirectionToPlayerCamera()
    {
        Quaternion rotatedQuat = Quaternion.Euler(-playerTransform.eulerAngles);
        lightTransform.rotation = rotatedQuat;
    }
}
