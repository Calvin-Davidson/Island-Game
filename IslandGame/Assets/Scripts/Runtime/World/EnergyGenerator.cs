using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : MonoBehaviour
{
    [SerializeField] private WorldManager worldManager;
    [SerializeField] private int generateEachSeconds;

    private void Awake()
    {
        StartCoroutine(Generator());
    }

    private IEnumerator Generator()
    {
        while (true)
        {
            yield return new WaitForSeconds(generateEachSeconds);

            foreach (var tileData in worldManager.Hexagons.Values)
            {
                tileData.GenerateNewEnergy();
            }
        }
    }
}
