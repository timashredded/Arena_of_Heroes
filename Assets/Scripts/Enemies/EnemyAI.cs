using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float stopDistance = 1.2f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            MoveToPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    void MoveToPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        animator.SetFloat("Speed", moveSpeed);

        // Поворот к игроку
        if (direction.x > 0)
            spriteRenderer.flipX = true;
        else if (direction.x < 0)
            spriteRenderer.flipX = false;
    }

    void StopMoving()
    {
        animator.SetFloat("Speed", 0f);
    }
}
