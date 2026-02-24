using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Combat")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1.2f;
    public int damage = 10;

    [Header("Detection")]
    public float aggroRange = 8f;

    private float lastAttackTime;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isAttacking;

    private Vector3 currentSurroundPoint;
    private float surroundRefreshTime;

    Vector3 GetSurroundPosition()
    {
        float angle = Random.Range(0f, 360f);
        float radius = attackRange - 0.2f;

        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0
        ) * radius;

        Vector3 targetPos = player.position + offset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return player.position;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= aggroRange)
        {
            if (distance > attackRange)
            {
                agent.isStopped = false;
                if (Time.time > surroundRefreshTime)
                {
                    currentSurroundPoint = GetSurroundPosition();
                    surroundRefreshTime = Time.time + Random.Range(1f, 2f);
                }

                agent.SetDestination(currentSurroundPoint);
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
            else
            {
                agent.isStopped = true;
                animator.SetFloat("Speed", 0f);
                TryAttack();
            }
        }
        else
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0f);
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        animator.SetTrigger("Attack");

        // Здесь позже добавим урон игроку
    }
}