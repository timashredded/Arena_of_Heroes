using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float stopDistance = 1.2f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        moveDirection = (player.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Move();
        }
        else
        {
            Stop();
        }
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        animator.SetFloat("Speed", moveSpeed);

        // ������� � ������
        if (moveDirection.x > 0)
            spriteRenderer.flipX = true;
        else if (moveDirection.x < 0)
            spriteRenderer.flipX = false;
    }

    void Stop()
    {
        animator.SetFloat("Speed", 0f);
        rb.linearVelocity = Vector2.zero;
    }

}
