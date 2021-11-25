using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInput : MonoBehaviour
{
    [SerializeField] private InputBrain brain;
    [SerializeField] private CameraZoom cameraZoom;
    
    private void Awake()
    {
        InstantiatePinchDetection();
        InstantiateMouseScrollDetection();
    }

    private void InstantiatePinchDetection()
    {
        PinchDetection pinchDetection = GetComponent<PinchDetection>();
        if (pinchDetection == null)
        {
            pinchDetection = gameObject.AddComponent<PinchDetection>();
        }
        pinchDetection.Brain = brain;
        
        brain.InputActionAsset["SecondaryTouchContact"].started += _ => pinchDetection.ZoomStart();
        brain.InputActionAsset["SecondaryTouchContact"].canceled += _ => pinchDetection.ZoomEnd();

        pinchDetection.onPinch += cameraZoom.ZoomOut;
        pinchDetection.onStretch += cameraZoom.ZoomIn;
    }

    private void InstantiateMouseScrollDetection()
    {
        brain.InputActionAsset["MouseScroll"].performed += context =>
        {
            if (context.ReadValue<Vector2>().y > 0)
            {
                cameraZoom.ZoomIn();
            }
            else
            {
                cameraZoom.ZoomOut();
            }
        };
    }
}
