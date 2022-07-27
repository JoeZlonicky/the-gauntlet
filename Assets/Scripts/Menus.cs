using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Menus : MonoBehaviour
{
    public bool waitingForKeypressToContinue;
    
    private Animator _animator;
    private static readonly int GameOverTrigger = Animator.StringToHash("gameOverTrigger");
    private static readonly int KeyPressTrigger = Animator.StringToHash("keyPressTrigger");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (waitingForKeypressToContinue && Input.anyKey) {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("game_over")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else {
                _animator.SetTrigger(KeyPressTrigger);
            }
            
        }
    }
    
    public void GameOver()
    {
        _animator.SetTrigger(GameOverTrigger);
    }
}
