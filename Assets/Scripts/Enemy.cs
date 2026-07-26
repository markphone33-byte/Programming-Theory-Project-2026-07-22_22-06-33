using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;

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
            agent.SetDestination(player.position);
        }
    }
}
