using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    protected Transform player;
    protected NavMeshAgent agent;
    protected float nextUpdateTime;
    [SerializeField] protected float updateInterval = 0.5f;
    protected float moveSpeed;
    [SerializeField] protected float baseMoveSpeed = 3;
    protected EnemyStatus enemyStatusScript;
    protected bool canRetarget = true;
    [SerializeField] protected float continueChaseTime = 4;
    protected float chaseTime = 0;
    [SerializeField] protected float visionDistance = 50;
    [SerializeField] protected float wanderDistance = 50;
    protected NavMeshQueryFilter navMeshFilter;
    protected EnemyAnimation enemyAnimation;
    [SerializeField] protected float antiCirclingDistance = 5;
    [SerializeField] protected float antiCirclingRetargetTime = 0.1f;
    protected float lastUpdateTime;

    // Initializes variables
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        nextUpdateTime = Time.time;
        enemyStatusScript = GetComponent<EnemyStatus>();
        navMeshFilter = new NavMeshQueryFilter
        {
            agentTypeID = agent.agentTypeID,
            areaMask = agent.areaMask
        };
        enemyAnimation = GetComponent<EnemyAnimation>();
        moveSpeed = baseMoveSpeed;
    }
    // Initializes refereences to other game objects
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // If chasing the player and the player is very close then constantly retarget towards them. Makes it is harder to circle around the enemy
        if (enemyStatusScript.DistanceToPlayer() < antiCirclingDistance && chaseTime > 0 && nextUpdateTime > Time.time + antiCirclingRetargetTime)
        {
            nextUpdateTime = Time.time + antiCirclingRetargetTime;
        }

        // If the enemy stops moving, it will retarget
        if (agent.velocity == Vector3.zero && !enemyStatusScript.isStunned && canRetarget)
        {
            StartCoroutine(RetargetCooldown(0.1f));
            nextUpdateTime = Time.time;
        }

        // Every updateInterval seconds, the enemy will retarget
        if (Time.time >= nextUpdateTime && !enemyStatusScript.isStunned)
        {
            nextUpdateTime = Time.time + updateInterval;
            Movement();
            enemyAnimation.AnimateMovement(agent.speed, chaseTime > 0);
            lastUpdateTime = Time.time;
        }
    }

    protected virtual void Movement()
    {
        //If not in chase then wanders around the map
        if (enemyStatusScript.DistanceToPlayer() > visionDistance) // Player is outside vision radius
        {
            Wander();
        }
        else if (!PlayerIsReachable()) // Player is not reachable on NavMesh
        {
            Wander();
        }
        else if (!PlayerInSight() && chaseTime <= 0)
        {
            Wander();
        }

        if (PlayerInSight()) // Player is not behind the enemy nor behind a wall
        {
            ChasePlayer();
        }
        // Enemy will continue to chase a little bit after player is out of sight
        else if (chaseTime > 0)
        {
            ContinueChase();
        }
    }

    protected virtual void Wander()
    {
        // If the enemy is nearing its destination then randomly pick a new destination
        if (agent.remainingDistance <= agent.stoppingDistance + 1)
        {
            // NavMeshHit wanderHit; // Will store where the enemy's wander position
            Vector3 wanderPosition = transform.position + Random.insideUnitSphere * wanderDistance; // Picks a random position within 50 units of the enemy's current position 
            wanderPosition.y = transform.position.y; // The map has no variation in height
            // NavMesh.SamplePosition(wanderPosition, out wanderHit, wanderDistance, NavMesh.AllAreas); // Finds the nearest valid position on the NavMesh to the random position and places it in wanderHit
            // agent.SetDestination(wanderHit.position);
            GoToPosition(wanderPosition, wanderDistance);
            agent.speed = moveSpeed;
        }
    }

    protected bool PlayerInSight()
    {
        // If player is just circling around the enemy, they are still counted as in line of sight
        if (enemyStatusScript.DistanceToPlayer() < antiCirclingDistance && chaseTime > 0)
        {
            return true;
        }

        Vector3 directionOfPlayer = (player.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionOfPlayer) <= 90) // Player is not behind the enemy
        {
            // Sends a ray from the enemy towards the player to check for any walls inbetween
            RaycastHit hit;
            Vector3 direction = player.position - transform.position;
            LayerMask wallsAndPlayer = LayerMask.GetMask("Wall", "Player");
            Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude, wallsAndPlayer);

            if (hit.transform == player) // Player is not behind a wall
            {
                return true;
            }
        }
        return false;
    }

    protected virtual IEnumerator RetargetCooldown(float delay)
    {
        canRetarget = false;
        yield return new WaitForSeconds(delay);
        canRetarget = true;
    }

    public void GoToPosition(Vector3 position, float maxSampleDistance)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, maxSampleDistance, navMeshFilter))
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, hit.position, navMeshFilter, path))
            {
                agent.SetDestination(hit.position);
                nextUpdateTime = Time.time + updateInterval; // Pushes back update time so enemy doesn't retarget right away
            }
        }
    }

    protected virtual bool PlayerIsReachable()
    {
        NavMeshPath pathToPlayer = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, player.position, navMeshFilter, pathToPlayer);
        return pathToPlayer.status == NavMeshPathStatus.PathComplete;
    }

    protected virtual void ChasePlayer()
    {
        // When chasing the player enemy will predict where the player is headed and go there
        agent.SetDestination(player.position + player.GetComponent<Rigidbody>().linearVelocity / 2);
        chaseTime = continueChaseTime;
    }

    protected virtual void ContinueChase()
    {
        chaseTime -= Time.time - lastUpdateTime;
        agent.SetDestination(player.position);
        agent.speed = moveSpeed;
    }
}
