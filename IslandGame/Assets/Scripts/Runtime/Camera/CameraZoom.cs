using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed;
    
    private Transform _transform;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public void ZoomIn()
    {
        Vector3 targetPosition = _transform.position + _transform.forward;
        _transform.position = Vector3.Slerp(_transform.position, targetPosition, Time.deltaTime * zoomSpeed);
    }

    public void ZoomOut()
    {
        Vector3 targetPosition = _transform.position + -_transform.forward;
        _transform.position = Vector3.Slerp(_transform.position, targetPosition, Time.deltaTime * zoomSpeed);
    }
}
