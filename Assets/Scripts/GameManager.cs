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
    public EnemySpawner enemySpawner;
    public float gameLength = 300.0f;
    public float timeBetweenWaves = 5.0f;

    private float _timer;
    private bool _timerIsEnabled;
    private int _waveNumber;

    private void Start()
    {
        _timer = 0.0f;
        _waveNumber = 0;
        _timerIsEnabled = true;
        player.onDeath.AddListener(PlayerDeath);
        player.onDeathFinished.AddListener(PlayerDeathFinished);
        enemySpawner.finalWaveKilled.AddListener(FinalWaveCompleted);
    }

    private void Update()
    {
        if (_timerIsEnabled) {
            _timer += Time.deltaTime;
            string minutes = Math.Floor(_timer / 60).ToString("0");
            string seconds = Math.Floor(_timer % 60).ToString("00");
            timerUI.text = $"{minutes}:{seconds}";
            
            if (_timer >= gameLength) {
                _timer = gameLength;
                _timerIsEnabled = false;
                enemySpawner.SpawnFinalWave();
            }
            else {
                int latestWaveNumber = (int) Math.Floor(_timer / timeBetweenWaves);
                if (latestWaveNumber > _waveNumber) {
                    enemySpawner.SpawnWave(_timer);
                    _waveNumber = latestWaveNumber;
                }
            }
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

    private void FinalWaveCompleted()
    {
        menus.Victory();
    }
}
