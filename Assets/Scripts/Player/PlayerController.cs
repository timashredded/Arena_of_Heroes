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
        animator.SetFloat("Speed", input.magnitude);

        HandleAutoAttack();


    }

    void FixedUpdate()
    {
        if (isAttacking) return;
        if (input == Vector2.zero) return;

        Vector2 move = input.normalized * moveSpeed * Time.fixedDeltaTime;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(LayerMask.GetMask("Enemy", "Walls"));

        RaycastHit2D[] hits = new RaycastHit2D[4];

        Vector2 position = rb.position;

        // ---- MOVE X ----
        if (move.x != 0)
        {
            Vector2 moveX = new Vector2(move.x, 0);

            int countX = rb.Cast(moveX.normalized, filter, hits, Mathf.Abs(move.x) + 0.05f);

            if (countX == 0)
                position.x += move.x;
        }

        // ---- MOVE Y ----
        if (move.y != 0)
        {
            Vector2 moveY = new Vector2(0, move.y);

            int countY = rb.Cast(moveY.normalized, filter, hits, Mathf.Abs(move.y) + 0.05f);

            if (countY == 0)
                position.y += move.y;
        }

        rb.MovePosition(position);
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
