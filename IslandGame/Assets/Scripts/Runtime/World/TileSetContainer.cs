using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "World/TileSet", order = 1)]
public class TileSetContainer : ScriptableObject
{
    [SerializeField] private GameObject[] tiles;

    public GameObject GetRandom()
    {
        return tiles.Random();
    }
}
