using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInput : MonoBehaviour
{
    [SerializeField] private InputBrain brain;
    [SerializeField] private CameraZoom cameraZoom;
    [SerializeField] private CameraRotation cameraRotation;

    private bool _isPrimaryHolding = false;
    private void Awake()
    {
        InstantiatePinchDetection();
        InstantiateMouseScrollDetection();
        InstantiateRotationDetection();
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

    private void InstantiateRotationDetection()
    {
        brain.InputActionAsset["PrimaryFingerContact"].started += _ => _isPrimaryHolding = true;
        brain.InputActionAsset["PrimaryFingerContact"].canceled += _ => _isPrimaryHolding = false;
        
        brain.InputActionAsset["PrimaryFingerDelta"].performed += context =>
        {
            if (!_isPrimaryHolding) return;
           
            Vector2 direction = context.ReadValue<Vector2>();

            if (direction.x > 0)
            {
                cameraRotation.RotateToRight();
            }
            else if (direction.x < 0)
            {
                cameraRotation.RotateToLeft();
            }
        };
    }
    
}
