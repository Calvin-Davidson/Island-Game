using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    private Placeable _placeable = Placeable.Empty;
    private GameObject _tileObject = null;

    public Placeable Placeable
    {
        get => _placeable;
        set => _placeable = value;
    }

    public GameObject TileObject
    {
        get => _tileObject;
        set => _tileObject = value;
    }
}
