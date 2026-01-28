using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 20;

    private Collider2D hitbox;

    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false; // ВАЖНО: по умолчанию выключен
    }

    public void EnableHitbox()
    {
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
