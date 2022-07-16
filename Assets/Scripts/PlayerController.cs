using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = System.Diagnostics.Debug;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rollSpeed;
    
    private static readonly int AnimIsMoving = Animator.StringToHash("isMoving");
    private static readonly int AnimXVelocity = Animator.StringToHash("xVelocity");
    private static readonly int AnimRollTrigger = Animator.StringToHash("rollTrigger");
    private static readonly int AnimAttackTrigger = Animator.StringToHash("attackTrigger");
    private static readonly int AnimIsAttacking = Animator.StringToHash("isAttacking");
    
    private Rigidbody2D _rb;
    private Animator _animator;

    private bool _isRolling;
    private float _rollCooldown = 0.5f;
    private float _lastRollTime;
    private Vector2 _lastDirection = Vector2.right;

    private bool _isAttacking;
    private float _attackCooldown = 0.5f;
    private float _lastAttackTime;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_isRolling && Input.GetKeyDown(KeyCode.Space)) {
            if (Time.time - _lastRollTime > _rollCooldown) {
                _lastRollTime = Time.time;
                _isRolling = true;
                _animator.SetTrigger(AnimRollTrigger);
            }
        }

        if (!_isRolling && Input.GetMouseButtonDown(0)) {
            if (Time.time - _lastAttackTime > _attackCooldown) {
                _lastAttackTime = Time.time;
                _isAttacking = true;
                _animator.SetTrigger(AnimAttackTrigger);
                _animator.SetBool(AnimIsAttacking, _isAttacking);
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

    public void AttackAnimationEnded()
    {
        _isAttacking = false;
        _animator.SetBool(AnimIsAttacking, _isAttacking);
    }
}
