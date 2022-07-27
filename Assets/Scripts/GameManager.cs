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

    private float _timer;

    private void Start()
    {
        _timer = 0.0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        string minutes = Math.Floor(_timer / 60).ToString("00");
        string seconds = Math.Floor(_timer % 60).ToString("00");
        timerUI.text = $"{minutes}:{seconds}";
    }
}
