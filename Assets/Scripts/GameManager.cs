using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerUI;
    public PlayerController player;
    public Menus menus;

    private float _timer;
    private bool _timerIsEnabled;

    private void Start()
    {
        _timer = 0.0f;
        _timerIsEnabled = true;
        player.onDeath.AddListener(PlayerDeath);
        player.onDeathFinished.AddListener(PlayerDeathFinished);
    }

    private void Update()
    {
        if (_timerIsEnabled) {
            _timer += Time.deltaTime;
            string minutes = Math.Floor(_timer / 60).ToString("0");
            string seconds = Math.Floor(_timer % 60).ToString("00");
            timerUI.text = $"{minutes}:{seconds}";
        }
    }

    private void PlayerDeath()
    {
        Time.timeScale = 0.4f;
        _timerIsEnabled = false;
    }

    private void PlayerDeathFinished()
    {
        Time.timeScale = 1.0f;
        menus.GameOver();
    }
}
