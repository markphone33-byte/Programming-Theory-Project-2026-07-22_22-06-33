using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatus : MonoBehaviour
{
    private NavMeshAgent agent;
    public bool isStunned { get; private set; } = false;
    private Transform player;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public void StunEnemy(float duration)
    {
        StartCoroutine(Stun(duration));
    }

    private IEnumerator Stun(float duration)
    {
        isStunned = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
        agent.isStopped = false;
    }

    // Gets the x and z distance between the enemy and the player ignoring height differences
    public float DistanceToPlayer()
    {
        float deltaX = Mathf.Pow(player.position.x - transform.position.x, 2);
        float deltaZ = Mathf.Pow(player.position.z - transform.position.z, 2);
        return Mathf.Sqrt(deltaX + deltaZ);
    }
}
