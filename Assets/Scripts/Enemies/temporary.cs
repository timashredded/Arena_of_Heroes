using UnityEngine;
using Pathfinding;

public class EnemyManualTarget : MonoBehaviour
{
    private AIPath ai;
    private Transform player;

    void Start()
    {
        ai = GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ai.destination = player.position;
    }

    void Update()
    {
        if (player != null)
            ai.destination = player.position;
    }
}
