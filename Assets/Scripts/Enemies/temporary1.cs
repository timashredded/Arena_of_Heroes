using UnityEngine;
using Pathfinding;

public class ForceDestination : MonoBehaviour
{
    private AIPath ai;
    private Transform player;

    void Start()
    {
        ai = GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            ai.destination = player.position;
            ai.SearchPath();
        }
    }
}
