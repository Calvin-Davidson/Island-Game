using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WorldBuilder))]
public class WorldManager : MonoBehaviour
{
    private static WorldManager _instance;
    
    [SerializeField] private TileSetContainer grassTiles;
    [SerializeField] private TileSetContainer sandTiles;

    private Dictionary<Vector2, TileData> _hexagons = new Dictionary<Vector2, TileData>();
    private WorldBuilder _worldBuilder;

    
    private void Awake()
    {
        _worldBuilder = GetComponent<WorldBuilder>();
        
        GameObject[] foundHexagons = GameObject.FindGameObjectsWithTag("HexagonTile");
        foreach (var o in foundHexagons)
        {
            Vector2 key = new Vector2(o.transform.position.x, o.transform.position.z);
            if (_hexagons.ContainsKey(key)) continue;

            TileData tileData = new TileData {TileObject = o};
            _hexagons.Add(key, tileData);
        }
    }
    
    public TileData GetTileFromPosition(Vector2 position)
    {
        return _hexagons.OrderBy(pair => Vector2.Distance(pair.Key, position)).FirstOrDefault().Value;
    }
    public bool IsEmptyTile(Vector2 position)
    {
        return !_hexagons.ContainsKey(position);
    }
    
    public bool ExpandIsland()
    {
        Vector2[] keys = _hexagons.Keys.ToArray();
        keys = keys.Shuffle();
        // shuffle
        foreach (var hexagonsKey in keys)
        {
            Vector2 leftBottom = GetBottomLeftCoords(hexagonsKey);
            Vector2 rightBottom = GetBottomRightCoords(hexagonsKey);
            Vector2 bottom = GetBottomCoords(hexagonsKey);

            Vector2 leftTop = GetTopLeftCoords(hexagonsKey);
            Vector2 rightTop = GetTopRightCoords(hexagonsKey);
            Vector2 top = GetTopCoords(hexagonsKey);

            if (_worldBuilder.TryCreateTile(leftBottom)) return true;
            if (_worldBuilder.TryCreateTile(rightBottom)) return true;
            if (_worldBuilder.TryCreateTile(bottom)) return true;
            if (_worldBuilder.TryCreateTile(leftTop)) return true;
            if (_worldBuilder.TryCreateTile(rightTop)) return true;
            if (_worldBuilder.TryCreateTile(top)) return true;
        }

        return false;
    }
    
    
    public bool ExpandIsland(int tiles)
    {
        for (int i = 0; i < tiles; i++)
        {
            if (ExpandIsland() == false) return false;
        }
        return true;
    }
    
    public static WorldManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WorldManager>();
                if (_instance == null)
                {
                    throw new Exception("There is WorldManager in this scene, so the world manager can not be used!");
                }
            }

            return _instance;
        }
    }

    public TileData GetClosestTileFromPosition(Vector2 vector2)
    {
        return _hexagons.OrderBy(pair => Vector2.Distance(pair.Key, vector2)).FirstOrDefault().Value;
    }


    public Vector2 GetTopCoords(Vector2 currentTile) => currentTile + new Vector2(0, 2);
    public Vector2 GetTopLeftCoords(Vector2 currentTile) => currentTile +  new Vector2(-1.5f, 1);
    public Vector2 GetTopRightCoords(Vector2 currentTile) => currentTile + new Vector2(1.5f, 1);
    public Vector2 GetBottomCoords(Vector2 currentTile) => currentTile + new Vector2(0, -2);
    public Vector2 GetBottomLeftCoords(Vector2 currentTile) => currentTile + new Vector2(-1.5f, -1f);
    public Vector2 GetBottomRightCoords(Vector2 currentTile) => currentTile + new Vector2(1.5f, -1f);

    
    public TileSetContainer GrassTiles => grassTiles;

    public TileSetContainer SandTiles => sandTiles;

    public Dictionary<Vector2, TileData> Hexagons => _hexagons;
}