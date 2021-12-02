using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "Data/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    private int _energy;

    public event Action<int> OnScoreUpdate;

    public int Energy
    {
        get => _energy;
        set
        {
            _energy = value;
            OnScoreUpdate?.Invoke(_energy);
        }
    }
}
