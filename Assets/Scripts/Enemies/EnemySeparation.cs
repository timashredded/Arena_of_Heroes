using UnityEngine;

public class EnemySeparation : MonoBehaviour
{
    public float radius = 0.6f;
    public float force = 1.5f;

    void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.gameObject != gameObject)
            {
                Vector2 dir = transform.position - hit.transform.position;
                transform.position += (Vector3)(dir.normalized * force * Time.fixedDeltaTime);
            }
        }
    }
}
