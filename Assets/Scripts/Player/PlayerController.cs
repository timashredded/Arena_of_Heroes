using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Combat")]
    public float attackRange = 1.2f;
    public float attackCooldown = 0.8f;

    private float lastAttackTime;

    private Transform lastTarget;

    private Transform currentTarget;

    private bool isAttacking;
    private Animator animator;

    [Header("Movement")]
    public float moveSpeed = 4.2f;
    public float acceleration = 12f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 currentVelocity;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            SelectTarget();
        }


        // Ввод
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
        HandleMovementFlip();


        // Скорость для анимации
        animator.SetFloat("Speed", rb.linearVelocity.magnitude);

        HandleAutoAttack();


    }

    void FixedUpdate()
    {
        // ❗ Блок движения во время атаки
        if (isAttacking) return;

        Vector2 targetVelocity = input * moveSpeed;
        currentVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = currentVelocity;
    }

    void Attack()
    {
        isAttacking = true;

        rb.linearVelocity = Vector2.zero;

        RotateToTarget();

        animator.SetTrigger("Attack");

        GetComponentInChildren<PlayerAttack>().EnableHitbox();

        Invoke(nameof(EndAttack), 0.35f);
    }


    void EndAttack()
    {
        GetComponentInChildren<PlayerAttack>().DisableHitbox();
        isAttacking = false;
    }


    void RotateToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;

        if (direction.x > 0)
            spriteRenderer.flipX = false;
        else if (direction.x < 0)
            spriteRenderer.flipX = true;
    }
    void HandleMovementFlip()
    {
        if (isAttacking) return;

        if (input.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (input.x < -0.1f)
            spriteRenderer.flipX = true;
    }
    void SelectTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        Debug.Log("Ray hit: " + (hit.collider != null ? hit.collider.name : "NULL"));

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            if (lastTarget != null)
            {
                EnemyHighlight old = lastTarget.GetComponent<EnemyHighlight>();
                if (old != null)
                    old.Hide();
            }

            currentTarget = hit.collider.transform;
            lastTarget = currentTarget;

            HighlightTarget(currentTarget);
        }
    }



    void HighlightTarget(Transform target)
    {
        EnemyHighlight highlight = target.GetComponent<EnemyHighlight>();

        if (highlight != null)
        {
            highlight.Show();
        }
    }

    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }
    void RotateToTarget()
    {
        if (currentTarget == null) return;

        Vector2 direction = currentTarget.position - transform.position;

        if (direction.x > 0)
            spriteRenderer.flipX = false;
        else if (direction.x < 0)
            spriteRenderer.flipX = true;
    }
    void HandleAutoAttack()
    {
        if (currentTarget == null) return;

        EnemyHealth enemyHealth = currentTarget.GetComponent<EnemyHealth>();
        if (enemyHealth == null || enemyHealth.IsDead)
        {
            ClearTarget();
            return;
        }

        float distance = Vector2.Distance(transform.position, currentTarget.position);

        if (distance > attackRange)
            return;

        if (Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            lastAttackTime = Time.time;
            Attack();
        }
    }
    void ClearTarget()
    {
        if (currentTarget != null)
        {
            EnemyHighlight highlight = currentTarget.GetComponent<EnemyHighlight>();
            if (highlight != null)
                highlight.Hide();
        }

        currentTarget = null;
    }





}
