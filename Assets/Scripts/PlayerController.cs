using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Debug = System.Diagnostics.Debug;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 10;
    public float moveSpeed;
    public float rollSpeed;
    public new Camera camera; // For determining attack direction
    public ParticleSystem hitParticlesPrefab;
    
    private static readonly int AnimIsMoving = Animator.StringToHash("isMoving");
    private static readonly int AnimXVelocity = Animator.StringToHash("xVelocity");
    private static readonly int AnimRollTrigger = Animator.StringToHash("rollTrigger");
    private static readonly int AnimAttackTrigger = Animator.StringToHash("attackTrigger");
    private static readonly int AnimIsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int AnimForceLeftDirection = Animator.StringToHash("forceLeftDirection"); 
    private static readonly int AnimForceRightDirection = Animator.StringToHash("forceRightDirection");
    private static readonly int AnimDeathTrigger = Animator.StringToHash("deathTrigger");
    private static readonly int AnimHitTrigger = Animator.StringToHash("hitTrigger");
    
    private const float RollCooldown = 0.5f;
    private const float KnockbackRecoveryRate = 0.25f;
    private const float AttackCooldown = 0.5f;
    private const float HitInvincibilityTime = 0.5f;

    private Rigidbody2D _rb;
    private Animator _animator;

    private int _health;
    private bool _isRolling;
    private float _lastRollTime;
    private bool _isAttacking;
    private float _lastAttackTime;
    private float _lastTimeHit;
    
    private Vector2 _lastDirection = Vector2.right;
    private Vector2 _knockback;
    [HideInInspector] public bool isDead;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = maxHealth;
    }

    private void Update()
    {
        // Can't attack or roll while doing either
        if (_isAttacking || _isRolling || isDead) {
            return;
        }
        
        // Roll
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Time.time - _lastRollTime > RollCooldown) {
                _lastRollTime = Time.time;
                _isRolling = true;
                _animator.SetTrigger(AnimRollTrigger);
            }
        }

        // Attack
        if (Input.GetMouseButtonDown(0)) {
            if (Time.time - _lastAttackTime > AttackCooldown) {
                _lastAttackTime = Time.time;
                _isAttacking = true;
                _animator.SetTrigger(AnimAttackTrigger);
                _animator.SetBool(AnimIsAttacking, _isAttacking);
                FaceAttackDirection();
            }
        }
    }
    private void FixedUpdate()
    {
        if (isDead) {
            _rb.velocity = Vector2.zero;
            _animator.SetTrigger(AnimDeathTrigger);  // Fix bug where animation doesn't play if during a transition
            return;
        }
        
        Vector2 movement;
        if (_isRolling) {
            movement = _lastDirection.normalized * rollSpeed;
        }
        else {
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");
            movement = new Vector2(xInput, yInput).normalized * moveSpeed;
        }
        
        movement += _knockback;
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, KnockbackRecoveryRate);
        
        _animator.SetBool(AnimIsMoving,  _rb.velocity.magnitude > 0.1);
        _animator.SetFloat(AnimXVelocity, _rb.velocity.x);

        if (!_isRolling && movement.magnitude > 0.1) {
            _lastDirection = movement.normalized;
        }
        
        _rb.velocity = movement;
    }

    private void FaceAttackDirection()
    {
        _animator.ResetTrigger(AnimForceRightDirection);
        _animator.ResetTrigger(AnimForceLeftDirection);
        float mouseDistance = camera.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        if (mouseDistance >= 0.0) {
            _animator.SetTrigger(AnimForceRightDirection);
        }
        else {
            _animator.SetTrigger(AnimForceLeftDirection);
        }
    }

    public void TakeDamage(int amount, Vector2 hitKnockback = default)
    {
        if (Time.time - _lastTimeHit < HitInvincibilityTime || isDead || _isRolling) {
            return;
        }
        
        _lastTimeHit = Time.time;
        _health -= amount;
        Instantiate(hitParticlesPrefab, transform);
        _animator.SetTrigger(AnimHitTrigger);
        if (_health <= 0) {
            isDead = true;
            _animator.SetTrigger(AnimDeathTrigger);
        }
        else {
            _knockback = hitKnockback;
        }
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

    public void DeathFinished()
    {
        Destroy(gameObject);
    }
}
