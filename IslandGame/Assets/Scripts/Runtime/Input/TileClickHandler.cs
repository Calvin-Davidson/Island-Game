using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileClickHandler : MonoBehaviour
{
    [SerializeField] private InputBrain inputBrain;
    [SerializeField] private UnityEvent<GameObject> onTileClick;

    public UnityEvent<GameObject> ONTileClick => onTileClick;

    private void Start()
    {
        inputBrain.InputActionAsset["PrimaryFingerContact"].performed += _ =>
        {
            Vector2 clickPosition = inputBrain.InputActionAsset["PrimaryFingerPosition"].ReadValue<Vector2>();
            Vector3 worldPosition;

            var ray = Camera.main.ScreenPointToRay(clickPosition);

            if (Physics.Raycast(ray, out var hit))
            {
                if (!hit.collider.gameObject.CompareTag("HexagonTile")) return;

                worldPosition = hit.point;

                GameObject obj = WorldManager.Instance.GetClosestTileFromPosition(new Vector2(worldPosition.x, worldPosition.z));
                onTileClick?.Invoke(obj);

                WorldManager.Instance.ExpandIsland(50);
            }
        };
    }
}