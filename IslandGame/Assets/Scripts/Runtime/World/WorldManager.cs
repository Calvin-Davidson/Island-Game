using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private static WorldManager _instance;

    private Dictionary<Vector2, GameObject> _hexagons = new Dictionary<Vector2, GameObject>();


    private void Awake()
    {
        GameObject[] foundHexagons = GameObject.FindGameObjectsWithTag("HexagonTile");
        foreach (var o in foundHexagons)
        {
            Vector2 key = new Vector2(o.transform.position.x, o.transform.position.z);
            if (_hexagons.ContainsKey(key)) continue;

            _hexagons.Add(key, o);
        }
    }

    public GameObject GetTileFromPosition(Vector2 position)
    {
        return _hexagons.OrderBy(pair => Vector2.Distance(pair.Key, position)).FirstOrDefault().Value;
    }

    public bool SpawnTile(Vector2 tileToSpawn)
    {
        if (_hexagons.ContainsKey(tileToSpawn)) return false;

        Mesh hexagonMesh = HexagonMeshBuilder.GetHexagonMesh(1, 1);
        GameObject hexTile = new GameObject("Hexagon", typeof(MeshRenderer), typeof(MeshFilter));
        hexTile.transform.position = new Vector3(tileToSpawn.x, 0, tileToSpawn.y);
        hexTile.GetComponent<MeshFilter>().mesh = hexagonMesh;
        _hexagons.Add(tileToSpawn, hexTile);
        return true;
    }

    public bool IsEmptyTile(Vector2 position)
    {
        return !_hexagons.ContainsKey(position);
    }

    public bool ExpandIsland()
    {
        Vector2[] keys = _hexagons.Keys.ToArray();
        // shuffle
        foreach (var hexagonsKey in keys)
        {
            Vector2 leftBottom = hexagonsKey + new Vector2(-1.5f, -1f);
            Vector2 rightBottom = hexagonsKey + new Vector2(1.5f, 1);
            Vector2 bottom = hexagonsKey + new Vector2(0, -2);

            Vector2 leftTop = hexagonsKey + new Vector2(-1.5f, 1);
            Vector2 rightTop = hexagonsKey + new Vector2(1.5f, 1);
            Vector2 top = hexagonsKey + new Vector2(0, 2);

            if (TrySpawn(leftBottom)) return true;
            if (TrySpawn(rightBottom)) return true;
            if (TrySpawn(bottom)) return true;
            if (TrySpawn(leftTop)) return true;
            if (TrySpawn(rightTop)) return true;
            if (TrySpawn(top)) return true;
        }

        return false;
    }

    private bool TrySpawn(Vector2 position)
    {
        if (IsEmptyTile(position))
        {
            Debug.Log("spawning at position " + position);
            SpawnTile(position);
            return true;
        }

        return false;
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

    public GameObject GetClosestTileFromPosition(Vector2 vector2)
    {
        return _hexagons.OrderBy(pair => Vector2.Distance(pair.Key, vector2)).FirstOrDefault().Value;
    }
}