using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage;
    public float knockbackAmount;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<EnemyController>(out var enemy)) return;

        Vector2 toEnemy = enemy.transform.position - transform.position;
        Vector2 knockback = toEnemy.normalized * knockbackAmount;
        enemy.TakeDamage(damage, knockback);
    }
}
