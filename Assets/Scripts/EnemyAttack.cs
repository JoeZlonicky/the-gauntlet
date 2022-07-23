using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    public float knockbackAmount;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Attack(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        Attack(col);
    }

    private void Attack(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")) {
            PlayerController player = col.transform.GetComponent<PlayerController>();
            Vector2 knockback = (player.transform.position - transform.position).normalized * knockbackAmount;
            player.TakeDamage(damage, knockback);
        }
    }
}
