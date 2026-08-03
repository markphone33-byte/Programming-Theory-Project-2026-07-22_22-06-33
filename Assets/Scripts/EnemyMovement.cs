using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private float nextUpdateTime;
    [SerializeField] private float updateInterval = 1;
    [SerializeField] private float moveSpeed;
    private EnemyAttack enemyAttackScript;

    // Initializes variables
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        nextUpdateTime = Time.time;
        enemyAttackScript = GetComponent<EnemyAttack>();
    }
    // Initializes refereences to other game objects
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // If the enemy reaches it's destination, it will retarget
        if (agent.remainingDistance < 0.1f && !enemyAttackScript.isStunned)
        {
            nextUpdateTime = Time.time;
        }

        // Every updateInterval seconds, the enemy will retarget
        if (Time.time >= nextUpdateTime && !enemyAttackScript.isStunned)
        {
            nextUpdateTime = Time.time + updateInterval;
            Movement();
        }
    }

    private void Movement()
    {
        // Chases the player with increased speed if they are within 70 units
        if (agent.path.status == NavMeshPathStatus.PathComplete && enemyAttackScript.DistanceToPlayer() < 70)
        {
            agent.SetDestination(player.position);
            agent.speed = moveSpeed * 1.5f;
        }
        //Otherwise wanders around the map at normal speed
        else
        {
            Wander();
            nextUpdateTime += 7; // Enemy wanders for a longer period of time before retargeting
            agent.speed = moveSpeed;
        }
    }

    private void Wander()
    {
        NavMeshHit wanderHit; // Will store where the enemy's wander position
        Vector3 wanderPosition = transform.position + Random.insideUnitSphere * 50; // Picks a random position within 50 units of the enemy's current position 
        NavMesh.SamplePosition(wanderPosition, out wanderHit, 30, NavMesh.AllAreas); // Finds the nearest valid position on the NavMesh to the random position and places it in wanderHit
        agent.SetDestination(wanderHit.position);
    }

}
