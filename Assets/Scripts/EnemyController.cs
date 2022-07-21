using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public PlayerController player;
    
    private float speed = 2.0f;
    private Rigidbody2D _rb;
    private Vector2 _knockback;

    private int _maxHealth = 10;
    private int _health;
    
    
    public void TakeDamage(int amount, Vector2 hitKnockback = default)
    {
        _health -= amount;
        if (_health <= 0) {
            Destroy(gameObject);
        }
        else {
            _knockback = hitKnockback;
        }
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = _maxHealth;
    }

    private void FixedUpdate()
    {
        Vector2 movement = default;

        if (player) {
            movement = (player.transform.position - transform.position).normalized * speed;
        }
        
        movement += _knockback;
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, 0.5f);
        
        _rb.velocity = movement;
    }
}
