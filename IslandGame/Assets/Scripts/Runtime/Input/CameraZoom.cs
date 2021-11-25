using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private InputBrain inputBrain;
    [SerializeField] private float zoomSpeed;
    private void Start()
    {
        inputBrain.InputActionAsset["Zoom"].performed += context =>
        {
            Vector2 zoomDelta = context.ReadValue<Vector2>();
            if (zoomDelta.y > 0) ZoomIn();
            else ZoomOut();
        };
    }

    private void ZoomIn()
    {
        Camera.main.transform.Translate(Camera.main.transform.right * Time.deltaTime * zoomSpeed);   
    }

    private void ZoomOut()
    {
        Camera.main.transform.Translate(-Camera.main.transform.right * Time.deltaTime * zoomSpeed); 
    }

}
