using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    public bool isStunned {get; private set; } = false;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float damage = 10f;
    private float stunDuration;

    // Initializes variables
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stunDuration = 1f;
    }
    // Initializes refereences to other game objects
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (DistanceToPlayer() < attackRange && !isStunned)
        {
            Attack();
        }
    }

    private void Attack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage);

        // Enemy is stunned for 1 second after hitting the player
        StartCoroutine(Stun(stunDuration));
    }

    // Gets the x and z distance between the enemy and the player ignoring height differences
    public float DistanceToPlayer()
    {
        float deltaX = Mathf.Pow(player.position.x - transform.position.x, 2);
        float deltaZ = Mathf.Pow(player.position.z - transform.position.z, 2);
        return Mathf.Sqrt(deltaX + deltaZ);
    }

    public IEnumerator Stun(float duration)
    {
        isStunned = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
        agent.isStopped = false;
    }
}
