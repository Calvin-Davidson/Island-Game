using Unity.Mathematics;
using UnityEngine;

public class PlaceHandler : MonoBehaviour
{
    [SerializeField] private Placeable selectedPlaceable;
    [SerializeField] private TileClickHandler tileClickHandler;
    [SerializeField] private PlaceableParser placeableParser;
    
    private void Awake()
    {
        tileClickHandler.OnClickEmptyTile.AddListener(SpawnSelected);
    }

    public Placeable SelectedPlaceable
    {
        get => selectedPlaceable;
        set
        {
            if (value == Placeable.Empty) tileClickHandler.OnClickEmptyTile.RemoveListener(SpawnSelected);
            if (selectedPlaceable == Placeable.Empty && value != Placeable.Empty) tileClickHandler.OnClickEmptyTile.AddListener(SpawnSelected);
            selectedPlaceable = value;
        }
    }
    private void SpawnSelected(TileData tileData)
    {
        if (tileData.Placeable != Placeable.Empty) return;
        
        Vector3 spawnPosition = tileData.TileObject.transform.position;
        spawnPosition.y = placeableParser.Parse(selectedPlaceable).transform.position.y;
        tileData.Placeable = Placeable.MainTower;
        
        Instantiate(placeableParser.Parse(selectedPlaceable), spawnPosition, quaternion.identity);
    }
}
