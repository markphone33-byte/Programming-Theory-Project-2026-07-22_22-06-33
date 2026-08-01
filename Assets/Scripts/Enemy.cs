using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework.Internal;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private float nextUpdateTime;
    [SerializeField] private float updateInterval = 1;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        nextUpdateTime = Time.time;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            nextUpdateTime = Time.time + updateInterval;
            enemyMovement();
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log(nextUpdateTime - Time.time);
        }
    }

    void enemyMovement()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        agent.SetDestination(player.position);
        if (agent.path.status == NavMeshPathStatus.PathComplete && distance < 70)
        {
            agent.speed = 8;
            Debug.Log("Chase");
        }
        else
        {
            NavMeshHit wanderHit;
            Vector3 wanderPosition = transform.position + Random.insideUnitSphere * 50;
            NavMesh.SamplePosition(wanderPosition, out wanderHit, 30, NavMesh.AllAreas);
            agent.SetDestination(wanderHit.position);
            nextUpdateTime += 3;
            agent.speed = 4;
            Debug.Log("Wander");
        }
    }
}
