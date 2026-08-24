using Unity.VisualScripting;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSourceValues footstepAudioValues;
    [SerializeField] private AudioSourceValues gruntAudioValues;
    [SerializeField] private AudioSourceValues talkAudioValues;
    private float nextUpdateTime;
    [SerializeField] private float updateInterval = 2;
    [SerializeField] private float updateIntervalRange = 2;
    private Transform player;
    private EnemyAttack enemyAttackScript;

    void Awake()
    {
        nextUpdateTime = Time.time;
        enemyAttackScript = GetComponent<EnemyAttack>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (nextUpdateTime < Time.time)
        {
            nextUpdateTime += updateInterval + Random.Range(-updateIntervalRange, updateIntervalRange);
            if (enemyAttackScript.EnemyDistanceToPlayer() < talkAudioValues.GetMaxDistance())
            {
                if (!PlayerCanSee())
                {
                    PlayTalkSoud();
                }
            }
        }
    }

    public void PlayFootstepSound()
    {
        footstepAudioValues.PlayRandomClip();
    }

    public void PlayGruntSoud()
    {
        gruntAudioValues.PlayRandomClip();
    }

    private void PlayTalkSoud()
    {
        talkAudioValues.PlayRandomClip();
    }

    private bool PlayerCanSee()
    {
        Vector3 directionOfEnemy = (transform.position - player.position).normalized;
        if (Vector3.Angle(player.forward, directionOfEnemy) <= 45) // Enemy is not behind the player
        {
            // Sends a ray from the enemy towards the player to check for any walls inbetween
            RaycastHit hit;
            Vector3 direction = player.position - transform.position;
            LayerMask wallsAndPlayer = LayerMask.GetMask("Wall", "Player");
            Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude, wallsAndPlayer);

            if (hit.transform == player) // Enemy is not behind a wall
            {
                return true;
            }
        }
        return false;
    }
}
