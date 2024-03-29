using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 10;
    public float moveSpeed;
    public float rollSpeed;
    public new Camera camera; // For determining attack direction
    public HealthBar healthBar;
    public ParticleSystem hitParticlesPrefab;
    public AudioSource swingSfx;
    public AudioSource swingSfx2;
    public AudioSource rollSfx;

    public UnityEvent onDeath;
    public UnityEvent onDeathFinished;
    public UnityEvent onMovementKeyUsed;
    public UnityEvent onLeftAttackUsed;
    public UnityEvent onRightAttackUsed;
    public UnityEvent onRollUsed;
    
    private static readonly int AnimIsMoving = Animator.StringToHash("isMoving");
    private static readonly int AnimXVelocity = Animator.StringToHash("xVelocity");
    private static readonly int AnimRollTrigger = Animator.StringToHash("rollTrigger");
    private static readonly int AnimAttackTrigger = Animator.StringToHash("attackTrigger");
    private static readonly int AnimIsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int AnimIsRolling = Animator.StringToHash("isRolling");
    private static readonly int AnimForceLeftDirection = Animator.StringToHash("forceLeftDirection"); 
    private static readonly int AnimForceRightDirection = Animator.StringToHash("forceRightDirection");
    private static readonly int AnimDeathTrigger = Animator.StringToHash("deathTrigger");
    private static readonly int AnimHitTrigger = Animator.StringToHash("hitTrigger");
    private static readonly int AnimIsMouseRightOfPlayer = Animator.StringToHash("isMouseRightOfPlayer");
    
    private const float RollCooldown = 0.5f;
    private const float KnockbackRecoveryRate = 0.15f;
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
    private bool _useSecondarySwingSfx;
    
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
        // Can't attack while rolling and vice-versa
        if (_isAttacking || _isRolling || isDead) {
            return;
        }
        
        // Roll
        if (Time.timeScale > 0f && Input.GetKeyDown(KeyCode.Space)) {
            if (Time.time - _lastRollTime > RollCooldown) {
                _lastRollTime = Time.time;
                _isRolling = true;
                _animator.SetTrigger(AnimRollTrigger);
                if (_lastDirection.x > 0f) {
                    _animator.SetTrigger(AnimForceRightDirection);
                } else if (_lastDirection.x < 0f) {
                    _animator.SetTrigger(AnimForceLeftDirection);
                }
                _animator.SetBool(AnimIsRolling, _isRolling);
                onRollUsed.Invoke();
                rollSfx.Play();
            }
        }

        // Attack
        if (Time.timeScale > 0f && Input.GetMouseButtonDown(0)) {
            if (Time.time - _lastAttackTime > AttackCooldown) {
                _lastAttackTime = Time.time;
                _isAttacking = true;
                _animator.SetTrigger(AnimAttackTrigger);
                _animator.SetBool(AnimIsAttacking, _isAttacking);
                if (_useSecondarySwingSfx) {
                    swingSfx2.Play();
                }
                else {
                    swingSfx.Play();
                }

                _useSecondarySwingSfx = !_useSecondarySwingSfx;
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
            if (movement.magnitude > 0.1) {
                onMovementKeyUsed.Invoke();
            }
        }
        
        if (!_isRolling) {
            if (movement.magnitude > 0.1) {
                _lastDirection = movement.normalized;
            }
            _animator.SetBool(AnimIsMouseRightOfPlayer, IsMouseRightOfPlayer());
            _animator.SetFloat(AnimXVelocity, movement.x);
        }
        
        movement += _knockback;
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, KnockbackRecoveryRate);
        
        _animator.SetBool(AnimIsMoving,  movement.magnitude > 0.1);

        _rb.velocity = movement;
    }

    private void FaceAttackDirection()
    {
        _animator.ResetTrigger(AnimForceRightDirection);
        _animator.ResetTrigger(AnimForceLeftDirection);
        if (IsMouseRightOfPlayer()) {
            _animator.SetTrigger(AnimForceRightDirection);
            onRightAttackUsed.Invoke();
        }
        else {
            _animator.SetTrigger(AnimForceLeftDirection);
            onLeftAttackUsed.Invoke();
        }
    }

    public bool TakeDamage(int amount, Vector2 hitKnockback = default)
    {
        if (Time.time - _lastTimeHit < HitInvincibilityTime || isDead || _isRolling) {
            return false;
        }
        
        _lastTimeHit = Time.time;
        _health = Math.Max(_health - amount, 0);
        healthBar.SetHearts(_health);
        
        Instantiate(hitParticlesPrefab, transform);
        _animator.SetTrigger(AnimHitTrigger);
        if (_health <= 0) {
            isDead = true;
            _animator.SetTrigger(AnimDeathTrigger);
            onDeath.Invoke();
        }
        else {
            _knockback = hitKnockback;
        }

        return true;
    }

    private bool IsMouseRightOfPlayer()
    {
        return camera.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x >= 0f;
    }
    
    public void RollAnimationEnded()
    {
        _isRolling = false;
        _animator.SetBool(AnimIsRolling, _isRolling);
    }

    public void AttackAnimationEnded()
    {
        _isAttacking = false;
        _animator.SetBool(AnimIsAttacking, _isAttacking);
    }

    public void DeathFinished()
    {
        onDeathFinished.Invoke();
    }

    public void DisableInput()
    {
        _rb.velocity = Vector2.zero;
        _animator.SetFloat(AnimXVelocity, 0f);
        _animator.SetBool(AnimIsMoving,  false);
        enabled = false;
    }
}
