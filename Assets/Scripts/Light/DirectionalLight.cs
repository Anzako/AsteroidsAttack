using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLight : MonoBehaviour
{
    private Transform lightTransform;
    [SerializeField] private Transform cameraTransform;

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
        lightTransform.rotation = cameraTransform.rotation;
    }
}
