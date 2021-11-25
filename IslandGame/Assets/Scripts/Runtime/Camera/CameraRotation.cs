using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform cameraParent;
    
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public void RotateToLeft()
    {
        Vector3 currentRotation = cameraParent.rotation.eulerAngles;
        currentRotation.y += rotationSpeed * Time.deltaTime;
        cameraParent.rotation = Quaternion.Euler(currentRotation);
    }

    public void RotateToRight()
    {
        Vector3 currentRotation = cameraParent.rotation.eulerAngles;
        currentRotation.y -= rotationSpeed * Time.deltaTime;
        cameraParent.rotation = Quaternion.Euler(currentRotation);        
    }
}
