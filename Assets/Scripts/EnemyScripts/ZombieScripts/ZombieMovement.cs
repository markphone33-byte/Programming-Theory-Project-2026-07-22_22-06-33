using System.Collections;
using UnityEngine;

public class ZombieMovement : EnemyMovement
{
    private bool isSlowlyAccelerating = false;
    [SerializeField] private float slowBaseMoveSpeed = 2;
    [SerializeField] private float slowAccelerationTime = 4;
    [SerializeField] protected float chaseSpeedBoost = 1.3f;

    protected override void Movement()
    {
        //If not in chase then wanders around the map at normal speed
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
            ReEnterChase(); // If the enemy is reentering chase then they will slowly accelerate
            ChasePlayer();
        }
        // Enemy will continue to chase a little bit after player is out of sight
        else if (chaseTime > 0)
        {
            ContinueChase();
        }
    }

    private void ReEnterChase()
    {
        if (!isSlowlyAccelerating && chaseTime > 0 && chaseTime < continueChaseTime)
        {
            StartCoroutine(SlowlyAccelerate(slowAccelerationTime));
        }
    }

    IEnumerator SlowlyAccelerate(float duration)
    {
        isSlowlyAccelerating = true;
        moveSpeed = slowBaseMoveSpeed;
        while (moveSpeed < baseMoveSpeed)
        {
            moveSpeed += (baseMoveSpeed - slowBaseMoveSpeed) * 0.8f * Time.deltaTime / duration;
            yield return new WaitForEndOfFrame();
        }
        moveSpeed = baseMoveSpeed;
        isSlowlyAccelerating = false;
    }

    protected override void ChasePlayer()
    {
        // When chasing the player enemy will predict where the player is headed and go there
        agent.SetDestination(player.position + player.GetComponent<Rigidbody>().linearVelocity / 2);
        agent.speed = moveSpeed * chaseSpeedBoost;
        chaseTime = continueChaseTime;
    }
}
