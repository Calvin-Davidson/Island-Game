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

    public void Rotate(Vector2 input)
    {
        Vector3 currentRotation = cameraParent.rotation.eulerAngles;
        currentRotation.y -= rotationSpeed * Time.deltaTime * input.x;
        cameraParent.rotation = Quaternion.Euler(currentRotation);             
    }
}
