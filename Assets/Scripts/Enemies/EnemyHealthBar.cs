using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;

    private EnemyHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    void Update()
    {
        if (enemyHealth == null) return;

        float percent = (float)enemyHealth.CurrentHealth / enemyHealth.MaxHealth;
        fillImage.fillAmount = percent;
    }
}
