using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rollSpeed;
    
    private static readonly int AnimIsMoving = Animator.StringToHash("isMoving");
    private static readonly int AnimXVelocity = Animator.StringToHash("xVelocity");
    private static readonly int AnimRollTrigger = Animator.StringToHash("rollTrigger");
    
    private Rigidbody2D _rb;
    private Animator _animator;

    private bool _isRolling;
    private float _rollCooldown = 0.5f;
    private float _lastRoll;
    private Vector2 _lastDirection = Vector2.right;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_isRolling && Input.GetKeyDown(KeyCode.Space)) {
            if (Time.time - _lastRoll > _rollCooldown) {
                _lastRoll = Time.time;
                _isRolling = true;
                _animator.SetTrigger(AnimRollTrigger);
            }
        }
    }
    private void FixedUpdate()
    {
        Vector2 movement;
        if (_isRolling) {
            movement = _lastDirection.normalized * rollSpeed;
        }
        else {
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");
            movement = new Vector2(xInput, yInput).normalized * speed;
        }
        
        _animator.SetBool(AnimIsMoving,  _rb.velocity.magnitude > 0.1);
        _animator.SetFloat(AnimXVelocity, _rb.velocity.x);

        if (!_isRolling && movement.magnitude > 0.1) {
            _lastDirection = movement.normalized;
        }
        
        _rb.velocity = movement;
    }

    public void RollAnimationEnded()
    {
        _isRolling = false;
    }
}
