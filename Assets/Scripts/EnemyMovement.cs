using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private float nextUpdateTime;
    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private float moveSpeed = 3;
    private EnemyAttack enemyAttackScript;
    private bool canRetarget = true;
    [SerializeField] private float continueChaseTime = 4;
    private float chaseTime = 0;

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
        // If the enemy stops moving, it will retarget
        if (agent.velocity == Vector3.zero && !enemyAttackScript.isStunned && canRetarget)
        {
            StartCoroutine(RetargetCooldown(0.1f));
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
        // Chases the player with increased speed under certain conditions
        if (agent.path.status == NavMeshPathStatus.PathComplete && enemyAttackScript.DistanceToPlayer() < 50) // Player is reachable and within 50 units
        {
            if (PlayerInSight()) // Player is not behind the enemy's line of sight or behind a wall
            {
                agent.SetDestination(player.position);
                agent.speed = moveSpeed * 1.3f;
                chaseTime = continueChaseTime;
                return; // Stops script here so enemy chases rather than wanders
            }
            // Enemy will continue to chase a little bit after player is out of sight, but does so at reduced speed
            else if (chaseTime > 0)
            {
                chaseTime -= updateInterval;
                agent.SetDestination(player.position);
                agent.speed = moveSpeed * 0.7f;
                return; // Stops script here so enemy chases rather than wanders
            }
        }
        Wander(); //If not in chase then wanders around the map at normal speed
    }

    private void Wander()
    {
        // If the enemy is nearing its destination then randomly pick a new destination
        if (agent.remainingDistance <= agent.stoppingDistance + 1)
        {
            NavMeshHit wanderHit; // Will store where the enemy's wander position
            Vector3 wanderPosition = transform.position + Random.insideUnitSphere * 50; // Picks a random position within 50 units of the enemy's current position 
            wanderPosition.y = transform.position.y; // The map has no variation in height
            NavMesh.SamplePosition(wanderPosition, out wanderHit, 30, NavMesh.AllAreas); // Finds the nearest valid position on the NavMesh to the random position and places it in wanderHit
            agent.SetDestination(wanderHit.position);
            agent.speed = moveSpeed;
        }
    }

    private bool PlayerInSight()
    {
        Vector3 directionOfPlayer = (player.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionOfPlayer) <= 90) // Player is not behind the enemy
        {
            // Sends a ray from the enemy towards the player to check for any walls inbetween
            RaycastHit hit;
            Vector3 direction = player.position - transform.position;
            LayerMask wallsAndPlayer = LayerMask.GetMask("Default", "Player");
            Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude, wallsAndPlayer);

            if (hit.transform == player) // Player is not behind a wall
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator RetargetCooldown(float delay)
    {
        canRetarget = false;
        yield return new WaitForSeconds(delay);
        canRetarget = true;
    }
}
