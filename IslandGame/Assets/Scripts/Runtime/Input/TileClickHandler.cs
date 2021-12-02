using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileClickHandler : MonoBehaviour
{
    [SerializeField] private InputBrain inputBrain;
    [SerializeField] private UnityEvent<TileData> onClickEmptyTile;
    [SerializeField] private UnityEvent<TileData> onClickFilledTile;

    public UnityEvent<TileData> OnClickEmptyTile => onClickEmptyTile;
    public UnityEvent<TileData> OnClickFilledTile => onClickFilledTile;

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

                TileData obj =
                    WorldManager.Instance.GetClosestTileFromPosition(new Vector2(worldPosition.x, worldPosition.z));

                if (obj.Placeable == Placeable.Empty) OnClickEmptyTile?.Invoke(obj);
                else OnClickFilledTile?.Invoke(obj);
            }
        };
    }
}