using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInput : MonoBehaviour
{
    [SerializeField] private InputBrain brain;
    [SerializeField] private CameraZoom cameraZoom;
    [SerializeField] private CameraRotation cameraRotation;

    private PinchDetection _pinchDetection;

    private bool _isPrimaryHolding = false;
    private void Awake()
    {
        InstantiatePinchDetection();
        InstantiateMouseScrollDetection();
        InstantiateRotationDetection();
    }

    private void InstantiatePinchDetection()
    {
        _pinchDetection = GetComponent<PinchDetection>();
        if (_pinchDetection == null)
        {
            _pinchDetection = gameObject.AddComponent<PinchDetection>();
        }
        _pinchDetection.Brain = brain;
        
        brain.InputActionAsset["SecondaryTouchContact"].started += _ => _pinchDetection.ZoomStart();
        brain.InputActionAsset["SecondaryTouchContact"].canceled += _ => _pinchDetection.ZoomEnd();

        _pinchDetection.onPinch += cameraZoom.ZoomOut;
        _pinchDetection.onStretch += cameraZoom.ZoomIn;
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
            if (_pinchDetection.IsDetectingZoom) return;
           
            Vector2 direction = context.ReadValue<Vector2>();

            cameraRotation.Rotate(direction);
        };
    }
    
}
