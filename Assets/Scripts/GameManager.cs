using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private bool _isPaused;
    private float _timeScaleBeforePause;

    private void Start()
    {
        _timer = 0.0f;
        _waveNumber = 0;
        player.onDeath.AddListener(PlayerDeath);
        player.onDeathFinished.AddListener(PlayerDeathFinished);
        enemySpawner.finalWaveKilled.AddListener(FinalWaveCompleted);
        Random.InitState(DateTime.Now.Millisecond);
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

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (_isPaused) {
                Time.timeScale = _timeScaleBeforePause;
                _isPaused = false;
                menus.Unpause();
            }
            else {
                _timeScaleBeforePause = Time.timeScale;
                Time.timeScale = 0f;
                _isPaused = true;
                menus.Pause();
            }
        }
    }

    public void StartTimer()
    {
        _timerIsEnabled = true;
    }

    private void PlayerDeath()
    {
        Time.timeScale = 0.4f;
        _timerIsEnabled = false;
    }

    private void PlayerDeathFinished()
    {
        Time.timeScale = 1f;
        menus.GameOver();
        player.DisableInput();
    }

    private void FinalWaveCompleted()
    {
        menus.Victory();
        player.DisableInput();
    }
}
