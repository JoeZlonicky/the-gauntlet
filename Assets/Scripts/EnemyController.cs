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
    
    private Rigidbody2D _rb;
    private Animator _animator;

    private const float Speed = 2.5f;
    private const int MaxHealth = 2;
    private const float KnockbackRecoveryRate = 0.15f;
    private const float MaxClosenessToPlayer = 0.35f;
    private int _health;
    private bool _dead = false;
    
    private Vector2 _knockback;
    
    public void TakeDamage(int amount, Vector2 hitKnockback = default)
    {
        _health -= amount;
        if (_health <= 0) {
            _dead = true;
            _animator.SetTrigger(AnimDeathTrigger);
        }
        else {
            _knockback = hitKnockback;
            _animator.SetTrigger(AnimHitTrigger);
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
        if (_dead) {
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
        
        _rb.velocity = movement;
    }
}
