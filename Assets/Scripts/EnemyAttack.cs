using UnityEngine;
using UnityEngine.Assertions;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    public float knockbackAmount;
    public AudioSource attackSFX;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Attack(collider.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        Attack(collider.gameObject);
    }

    private void Attack(GameObject gameObject)
    {
        if (!gameObject.TryGetComponent<PlayerController>(out var player)) return;

        Vector2 toPlayer = player.transform.position - transform.position;
        Vector2 knockback = toPlayer.normalized * knockbackAmount;

        bool attackSuccessful = player.TakeDamage(damage, knockback);

        // Don't want to play SFX if player is invulnerable to the damage
        if (attackSuccessful) {
            attackSFX.Play();
        }
    }
}
