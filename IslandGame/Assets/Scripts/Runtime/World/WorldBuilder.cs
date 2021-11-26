using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WorldManager))]
public class WorldBuilder : MonoBehaviour
{
    [SerializeField] private float popupAnimationSpeed;

    private WorldManager _worldManager;

    private void Awake()
    {
        _worldManager = GetComponent<WorldManager>();
    }

    public GameObject CreateTile(Vector2 spawnLocation)
    {
        if (_worldManager.Hexagons.ContainsKey(spawnLocation)) return null;

        GameObject hexTile = GameObject.Instantiate(_worldManager.GrassTiles.GetRandom());
        hexTile.transform.position = new Vector3(spawnLocation.x, 0, spawnLocation.y);
        _worldManager.Hexagons.Add(spawnLocation, hexTile);

        StartCoroutine(TilePopupAnimation(hexTile));
        return hexTile;
    }

    public GameObject TryCreateTile(Vector2 spawnLocation)
    {
        if (!_worldManager.IsEmptyTile(spawnLocation)) return null;
        return CreateTile(spawnLocation);
    }


    public bool DeleteTile(Vector3 spawnLocation)
    {
        if (!(_worldManager.Hexagons.ContainsKey(spawnLocation))) return false;

        GameObject obj = _worldManager.Hexagons[new Vector2(spawnLocation.x, spawnLocation.z)];
        GameObject.Destroy(obj);
        return true;
    }

    private IEnumerator TilePopupAnimation(GameObject tile)
    {
        Vector3 targetPosition = tile.transform.position;
        tile.transform.position = targetPosition - new Vector3(0, 1, 0);

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * popupAnimationSpeed;
            progress = progress > 1 ? 1 : progress;
            tile.transform.position = Vector3.Slerp(tile.transform.position, targetPosition, progress);
            yield return null;
        }
    }
}