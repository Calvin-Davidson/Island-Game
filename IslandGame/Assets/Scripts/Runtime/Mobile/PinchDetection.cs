using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    private InputBrain _brain;
    
    private Coroutine _zoomDetectionCoroutine;
    private Transform _cameraTransform;

    public Action onPinch;
    public Action onStretch;

    private void Awake()
    {
        if (Camera.main == null) throw new Exception("There is no main camera in the scene");
        _cameraTransform = Camera.main.transform;
    }
    
    public void ZoomStart()
    {
        GameObject.FindGameObjectWithTag("DebugCube").GetComponent<MeshRenderer>().material.color = Color.green;
        _zoomDetectionCoroutine = StartCoroutine(ZoomDetection());
    }

    public void ZoomEnd()
    {
        GameObject.FindGameObjectWithTag("DebugCube").GetComponent<MeshRenderer>().material.color = Color.red;
        StopCoroutine(_zoomDetectionCoroutine);
    }

    IEnumerator ZoomDetection()
    {
        float previousDistance = 0;
        float currentDistance = 0;

        while (true)
        {
            Vector2 primaryPosition = _brain.InputActionAsset["PrimaryFingerPosition"].ReadValue<Vector2>();
            Vector2 secondaryPosition = _brain.InputActionAsset["SecondaryFingerPosition"].ReadValue<Vector2>();

            currentDistance = Vector2.Distance(primaryPosition, secondaryPosition);

            // zoom out
            if (currentDistance > previousDistance)
            {
                onStretch?.Invoke();
            }
            // zoom in
            else if (currentDistance < previousDistance)
            {
                onPinch?.Invoke();
            }
            
            previousDistance = currentDistance;

            yield return null;
        }
    }

    public InputBrain Brain
    {
        get => _brain;
        set => _brain = value;
    }
}