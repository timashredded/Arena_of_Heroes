using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [Header("Combat")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1.2f;

    [Header("Detection")]
    public float aggroRange = 8f;

    private float lastAttackTime;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private static List<EnemyController> activeEnemies = new List<EnemyController>();

    void OnEnable()
    {
        activeEnemies.Add(this);
    }

    void OnDisable()
    {
        activeEnemies.Remove(this);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 2D NavMesh
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.stoppingDistance = 0f;
        agent.autoBraking = false;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > aggroRange)
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
            return;
        }

        if (distance > attackRange)
        {
            MoveToAssignedSlot();
        }
        else
        {
            Attack();
        }
    }

    void MoveToAssignedSlot()
    {
        agent.isStopped = false;

        List<EnemyController> nearbyEnemies = new List<EnemyController>();

        foreach (var enemy in activeEnemies)
        {
            if (enemy == null) continue;

            float dist = Vector2.Distance(enemy.transform.position, player.position);
            if (dist <= aggroRange)
                nearbyEnemies.Add(enemy);
        }

        int index = nearbyEnemies.IndexOf(this);
        int total = nearbyEnemies.Count;

        if (total == 0) return;

        float angle = index * Mathf.PI * 2f / total;
        float surroundRadius = attackRange;

        Vector3 desiredPos = player.position + new Vector3(
            Mathf.Cos(angle),
            Mathf.Sin(angle),
            0
        ) * surroundRadius;

        // 🔥 ПРОВЕРКА NAVMESH
        NavMeshHit hit;
        if (NavMesh.SamplePosition(desiredPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            agent.SetDestination(player.position);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
        HandleFlip();
    }
    void HandleFlipManual(Vector3 targetPos)
    {
        float dir = targetPos.x - transform.position.x;

        if (dir > 0.05f)
            spriteRenderer.flipX = false;
        else if (dir < -0.05f)
            spriteRenderer.flipX = true;
    }

    void Attack()
    {
        agent.isStopped = true;
        animator.SetFloat("Speed", 0);

        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
    }

    void HandleFlip()
    {
        Vector3 vel = agent.velocity;

        if (vel.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (vel.x < -0.1f)
            spriteRenderer.flipX = true;
    }
}