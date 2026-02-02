using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 20;

    private Collider2D hitbox;
    private CircleCollider2D circleCollider;
    private SpriteRenderer playerSprite;

    void Start()
    {
        playerSprite = GetComponentInParent<SpriteRenderer>();

        circleCollider = GetComponent<CircleCollider2D>();
        hitbox = circleCollider;

        hitbox.enabled = false;
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
        if (!other.CompareTag("Enemy")) return;

        Transform currentTarget = GetComponentInParent<PlayerController>().GetCurrentTarget();

        if (currentTarget == null) return;

        if (other.transform != currentTarget) return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }



}
