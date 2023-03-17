using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class Menus : MonoBehaviour
{
    public bool waitingForKeypressToContinue;
    
    private Animator _animator;
    private static readonly int GameOverTrigger = Animator.StringToHash("gameOverTrigger");
    private static readonly int KeyPressTrigger = Animator.StringToHash("keyPressTrigger");
    private static readonly int VictoryTrigger = Animator.StringToHash("victoryTrigger");
    private static readonly int PauseTrigger = Animator.StringToHash("pauseTrigger");
    private static readonly int UnpauseTrigger = Animator.StringToHash("unpauseTrigger");

    private static Menus _instance;  // Keep track to make sure duplicates are made with DontDestroyOnLoad

    public static Menus Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        _animator = GetComponent<Animator>();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (waitingForKeypressToContinue && Input.anyKey) {
            _animator.SetTrigger(KeyPressTrigger);
        }
    }
    
    public void GameOver()
    {
        _animator.SetTrigger(GameOverTrigger);
    }

    public void Victory()
    {
        _animator.SetTrigger(VictoryTrigger);
    }

    public void Pause()
    {
        _animator.SetTrigger(PauseTrigger);
    }

    public void Unpause()
    {
        _animator.SetTrigger(UnpauseTrigger);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
