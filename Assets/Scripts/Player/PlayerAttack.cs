using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 20;

    private CircleCollider2D circleCollider;
    private SpriteRenderer playerSprite;

    void Start()
    {
        playerSprite = GetComponentInParent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
    }

    void Update()
    {
        FlipHitbox();
    }

    void FlipHitbox()
    {
        Vector2 offset = circleCollider.offset;

        if (playerSprite.flipX)
            offset.x = -Mathf.Abs(offset.x);
        else
            offset.x = Mathf.Abs(offset.x);

        circleCollider.offset = offset;
    }

    // Вызвать через Animation Event
    public void EnableHitbox()
    {
        circleCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        circleCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}