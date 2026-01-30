using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public bool IsDead => isDead;

    public int maxHealth = 50;
    private int currentHealth;

    private Animator animator;

    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        animator.SetTrigger("Die");

        // ���������� AI
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null)
            ai.enabled = false;

        // ���������� ������
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // ��������� ��������� (����� �� ��������)
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // ������ HP bar
        Transform canvas = transform.Find("Canvas");
        if (canvas != null)
            canvas.gameObject.SetActive(false);

        // ���������� ����� ��������
        Destroy(gameObject, 1.2f);
    }



}
