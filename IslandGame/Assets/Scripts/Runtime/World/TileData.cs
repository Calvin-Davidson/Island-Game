using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    private Placeable _placeable = Placeable.Empty;
    private GameObject _tileObject = null;

    private int _energyStored = 0;
    private int _maxEnergy = 10;

    
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

    public int EnergyStored
    {
        get => _energyStored;
    }

    public void GenerateNewEnergy()
    {
        if (_maxEnergy == _energyStored) return;
        _energyStored += 1;
    }

    public void ClearEnergy()
    {
        _energyStored = 0;
    }
}
