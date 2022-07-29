using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    public Transform walkToTarget;
    public PlayerController player;
    public int maxHealth = 2;
    public float speed = 2.5f;
    public float knockbackRecoveryRate = 0.15f;
    public AudioSource hitSfx;
    public AudioSource deathSfx;

    public UnityEvent onDeath;
    public UnityEvent onDeathFinished;
    
    private static readonly int AnimHitTrigger = Animator.StringToHash("hitTrigger");
    private static readonly int AnimDeathTrigger = Animator.StringToHash("deathTrigger");
    private static readonly int AnimIsMoving = Animator.StringToHash("isMoving");
    private static readonly int AnimXVelocity = Animator.StringToHash("xVelocity");
    
    private Rigidbody2D _rb;
    private Animator _animator;
    public ParticleSystem hitParticlesPrefab;
    
    private const float MarginForReachingTarget = 0.2f;
    private int _health;
    private bool _isDead;
    private bool _playerDeathFinished;
    
    private Vector2 _knockback;
    
    public void TakeDamage(int amount, Vector2 hitKnockback = default)
    {
        if (_isDead) return;
        
        _health -= amount;
        Instantiate(hitParticlesPrefab, transform);
        _animator.SetTrigger(AnimHitTrigger);
        if (_health <= 0) {
            _isDead = true;
            _animator.SetTrigger(AnimDeathTrigger);
            onDeath.Invoke();
            deathSfx.Play();
        }
        else {
            _knockback = hitKnockback;
            hitSfx.Play();
        }
    }

    public void DeathFinished()
    {
        Destroy(gameObject);
        onDeathFinished.Invoke();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = maxHealth;
        player.onDeathFinished.AddListener(OnPlayerDeathFinished);
    }

    private void FixedUpdate()
    {
        if (_isDead || (walkToTarget == null && player == null || _playerDeathFinished)) {
            _rb.velocity = Vector2.zero;
            return;
        }
        
        Vector2 movement = default;

        Vector3 target = walkToTarget ? walkToTarget.position : player.transform.position;
        Vector2 vectorToTarget = (target - transform.position);
        
        if (vectorToTarget.magnitude > MarginForReachingTarget) {
            movement = vectorToTarget.normalized * speed;
            _animator.SetFloat(AnimXVelocity, movement.x);
        } else if (walkToTarget) {
            walkToTarget = null;
        }
        
        _animator.SetBool(AnimIsMoving, walkToTarget != null || player != null);

        movement += _knockback;
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, knockbackRecoveryRate);
        
        _rb.velocity = movement;
    }

    private void OnPlayerDeathFinished()
    {
        _playerDeathFinished = true;
        player.onDeathFinished.RemoveListener(OnPlayerDeathFinished);
    }
}
