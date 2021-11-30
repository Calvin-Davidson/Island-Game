using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlaceHandler : MonoBehaviour
{
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private TileClickHandler tileClickHandler;

    private void Awake()
    {
        tileClickHandler.OnClickEmptyTile.AddListener(SpawnSelected);
    }

    public GameObject SelectedObject
    {
        get => selectedObject;
        set
        {
            if (value == null) tileClickHandler.OnClickEmptyTile.RemoveListener(SpawnSelected);
            if (tileClickHandler == null && value != null) tileClickHandler.OnClickEmptyTile.AddListener(SpawnSelected);
            selectedObject = value;
        }
    }

    private void SpawnSelected(TileData tileData)
    {
        if (tileData.Placeable != Placeable.Empty) return;
        
        Vector3 spawnPosition = tileData.TileObject.transform.position;
        spawnPosition.y = selectedObject.transform.position.y;
        tileData.Placeable = Placeable.MainTower;
        
        Instantiate(selectedObject, spawnPosition, quaternion.identity);
    }
}
