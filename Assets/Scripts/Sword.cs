using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float knockbackAmount;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy")) {
            EnemyController enemy = col.transform.GetComponent<EnemyController>();
            Vector2 knockback = (enemy.transform.position - transform.position).normalized * knockbackAmount;
            enemy.TakeDamage(1, knockback);
        }
    }
}
