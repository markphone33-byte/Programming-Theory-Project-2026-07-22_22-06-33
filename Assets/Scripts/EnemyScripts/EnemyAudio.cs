using Unity.VisualScripting;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip[] gruntClips;
    [SerializeField] private AudioClip[] talkClips;
    [SerializeField] private AudioSource footstepsAudioSource;
    [SerializeField] private AudioSource gruntAudioSource;
    [SerializeField] private AudioSource talkAudioSource;
    [SerializeField] private float averagePitch = 1f;
    [SerializeField] private float pitchRange = 0.2f;
    [SerializeField] private float volume = 1;
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
            if (enemyAttackScript.EnemyDistanceToPlayer() < talkAudioSource.maxDistance)
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
        float averagePitch = 1f;
        float pitchRange = 0.2f;
        float volume = 0.4f;

        AudioClip randomClip = footstepClips[Random.Range(0, footstepClips.Length)];
        footstepsAudioSource.pitch = Random.Range(averagePitch - pitchRange, averagePitch + pitchRange);
        footstepsAudioSource.PlayOneShot(randomClip, volume);
    }

    public void PlayGruntSoud()
    {
        float averagePitch = 0.8f;
        float pitchRange = 0.1f;
        float volume = 0.2f;

        AudioClip randomClip = gruntClips[Random.Range(0, gruntClips.Length)];
        gruntAudioSource.pitch = Random.Range(averagePitch - pitchRange, averagePitch + pitchRange);
        gruntAudioSource.PlayOneShot(randomClip, volume);
    }

    private void PlayTalkSoud()
    {
        float averagePitch = 1f;
        float pitchRange = 0.2f;
        float volume = 0.2f;

        AudioClip randomClip = talkClips[Random.Range(0, talkClips.Length)];
        talkAudioSource.pitch = Random.Range(averagePitch - pitchRange, averagePitch + pitchRange);
        talkAudioSource.PlayOneShot(randomClip, volume);
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
