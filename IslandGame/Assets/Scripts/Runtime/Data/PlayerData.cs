using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "Data/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    private int _score;

    public event Action<int> OnScoreUpdate;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreUpdate?.Invoke(_score);
        }
    }
}
