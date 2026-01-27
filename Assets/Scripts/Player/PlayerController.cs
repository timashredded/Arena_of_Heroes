using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4.2f;
    public float acceleration = 12f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 currentVelocity;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        HandleFlip();
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = input * moveSpeed;
        currentVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = currentVelocity;
    }

    void HandleFlip()
    {
        // Если идём вправо
        if (input.x > 0.1f)
            spriteRenderer.flipX = false;

        // Если идём влево
        else if (input.x < -0.1f)
            spriteRenderer.flipX = true;
    }
}
