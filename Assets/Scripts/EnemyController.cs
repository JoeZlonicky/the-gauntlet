using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public PlayerController player;
    
    private static readonly int AnimHitTrigger = Animator.StringToHash("hitTrigger");
    private static readonly int AnimDeathTrigger = Animator.StringToHash("deathTrigger");
    private static readonly int AnimIsMoving = Animator.StringToHash("isMoving");
    private static readonly int AnimXVelocity = Animator.StringToHash("xVelocity");
    
    private Rigidbody2D _rb;
    private Animator _animator;

    private const float Speed = 2.5f;
    private const int MaxHealth = 2;
    private const float KnockbackRecoveryRate = 0.15f;
    private const float MaxClosenessToPlayer = 0.2f;
    private int _health;
    private bool _isDead;
    
    private Vector2 _knockback;
    
    public void TakeDamage(int amount, Vector2 hitKnockback = default)
    {
        if (_isDead) return;
        
        _health -= amount;
        _animator.SetTrigger(AnimHitTrigger);
        if (_health <= 0) {
            _isDead = true;
            _animator.SetTrigger(AnimDeathTrigger);
        }
        else {
            _knockback = hitKnockback;
        }
    }

    public void DeathFinished()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = MaxHealth;
    }

    private void FixedUpdate()
    {
        if (_isDead) {
            _rb.velocity = Vector2.zero;
            return;
        }
        
        Vector2 movement = default;

        if (player) {
            Vector2 vectorToPlayer = (player.transform.position - transform.position);
            if (vectorToPlayer.magnitude > MaxClosenessToPlayer) {
                movement = vectorToPlayer.normalized * Speed;
            }
            _animator.SetBool(AnimIsMoving, true);
        }
        else {
            _animator.SetBool(AnimIsMoving, false);
        }
        
        movement += _knockback;
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, KnockbackRecoveryRate);
        
        _animator.SetFloat(AnimXVelocity, _rb.velocity.x);
        _rb.velocity = movement;
    }
}
