using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 20;

    private Collider2D hitbox;
    private BoxCollider2D boxCollider;
    private SpriteRenderer playerSprite;

    void Start()
    {
        playerSprite = GetComponentInParent<SpriteRenderer>();

        boxCollider = GetComponent<BoxCollider2D>();
        hitbox = boxCollider;

        hitbox.enabled = false;
    }

    void Update()
    {
        FlipHitbox();
    }

    void FlipHitbox()
    {
        Vector2 offset = boxCollider.offset;

        if (playerSprite.flipX)
            offset.x = -Mathf.Abs(offset.x);
        else
            offset.x = Mathf.Abs(offset.x);

        boxCollider.offset = offset;
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
