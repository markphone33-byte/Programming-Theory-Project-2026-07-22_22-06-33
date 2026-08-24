using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float stunDuration;
    private EnemyAnimation enemyAnimationScript;
    private EnemyStatus enemyStatusScript;

    // Initializes variables
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        enemyAnimationScript = GetComponent<EnemyAnimation>();
        enemyStatusScript = GetComponent<EnemyStatus>();
    }
    // Initializes refereences to other game objects
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStatusScript.DistanceToPlayer() < attackRange && !enemyStatusScript.isStunned)
        {
            Attack();
        }
    }

    private void Attack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage);

        // Enemy is stunned for 1 second after hitting the player
        enemyStatusScript.StunEnemy(stunDuration);
        enemyAnimationScript.AnimateAttack();
    }
}
