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

        Vector2 dir = player.position - transform.position;

        animator.SetFloat("Speed", dir.magnitude);

        if (dir.x > 0)
            spriteRenderer.flipX = true;
        else if (dir.x < 0)
            spriteRenderer.flipX = false;
    }


    void Stop()
    {
        animator.SetFloat("Speed", 0f);
        rb.linearVelocity = Vector2.zero;
    }

}
