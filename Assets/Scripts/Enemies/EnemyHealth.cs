using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
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

        Destroy(gameObject, 1.2f); // под длину Death анимации
    }

}
