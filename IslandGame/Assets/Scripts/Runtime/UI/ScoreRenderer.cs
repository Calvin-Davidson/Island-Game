using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreRenderer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        playerData.OnScoreUpdate += SetScore;
    }

    public void SetScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
