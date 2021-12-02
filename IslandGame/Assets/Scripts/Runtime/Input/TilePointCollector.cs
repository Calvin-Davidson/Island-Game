using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePointCollector : MonoBehaviour
{
    [SerializeField] private TileClickHandler tileClickHandler;
    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        tileClickHandler.OnClickFilledTile.AddListener(CollectEnergy);
    }

    private void CollectEnergy(TileData data)
    {
        Debug.Log("collecting");
        Debug.Log(data.EnergyStored);
        playerData.Energy += data.EnergyStored;
        data.ClearEnergy();
    }
}