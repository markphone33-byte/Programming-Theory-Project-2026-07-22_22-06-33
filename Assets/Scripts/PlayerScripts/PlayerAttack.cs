using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private ParticleSystem slashParticle;
    private PlayerAudio playerAudioScript;
    private GameObject playerCamera;
    private Rigidbody playerRb;
    private LayerMask enemyLayer;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudioScript = GetComponent<PlayerAudio>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = GameObject.FindWithTag("MainCamera");
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    public void BasicMeleeAttack(float damage, Vector3 attackSize, float particleSpeed)
    {
        float particleLifetime = 0.1f;
        float particleSizeXMultiplier = 4f;
        float particleSizeYMultiplier = 1f;
        float particleSizeZMultiplier = 1f;

        BasicMeleeAttackParticle(attackSize.y, particleSpeed, particleLifetime, particleSizeXMultiplier, particleSizeYMultiplier, particleSizeZMultiplier);

        BasicMeleeAttackCollision(damage, attackSize);
    }

    public void BasicMeleeAttack(float damage, Vector3 attackSize, float particleSpeed, float particleLifetime)
    {
        float particleSizeXMultiplier = 4f;
        float particleSizeYMultiplier = 1f;
        float particleSizeZMultiplier = 1f;

        BasicMeleeAttackParticle(attackSize.y, particleSpeed, particleLifetime, particleSizeXMultiplier, particleSizeYMultiplier, particleSizeZMultiplier);

        BasicMeleeAttackCollision(damage, attackSize);
    }

    public void BasicMeleeAttack(float damage, Vector3 attackSize, float particleSpeed, float particleLifetime, float particleSizeXMultiplier, float particleSizeYMultiplier, float particleSizeZMultiplier)
    {
        BasicMeleeAttackParticle(attackSize.y, particleSpeed, particleLifetime, particleSizeXMultiplier, particleSizeYMultiplier, particleSizeZMultiplier);

        BasicMeleeAttackCollision(damage, attackSize);
    }

    private void BasicMeleeAttackParticle(float attackSizeY, float particleSpeed, float particleLifetime, float particleSizeXMultiplier, float particleSizeYMultiplier, float particleSizeZMultiplier)
    {
        float spawnForward = attackSizeY - 0.3f;
        float spawnForwardMomentum = 0.1f;

        // Rotated in the direction the player is facing. The added 90 is to correct the rotation to the be the middle of where the camera is facing
        Quaternion attackRotation = Quaternion.Euler(playerCamera.transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, 0);
        // Spawns in front of the player and a bit extra in front based on how fast the player is moving
        Vector3 attackSpawnPosition = playerCamera.transform.position + playerCamera.transform.forward * spawnForward + playerRb.linearVelocity * spawnForwardMomentum;

        ParticleSystem attackParticle = Instantiate(slashParticle, attackSpawnPosition, attackRotation);
        ParticleSystem.MainModule particleMain = attackParticle.main;
        particleMain.startLifetime = particleLifetime;
        particleMain.startSpeed = particleSpeed;
        particleMain.startSizeXMultiplier = particleSizeXMultiplier;
        particleMain.startSizeYMultiplier = particleSizeYMultiplier;
        particleMain.startSizeZMultiplier = particleSizeZMultiplier;
        attackParticle.Play();
    }

    private void BasicMeleeAttackCollision(float damage, Vector3 attackSize)
    {
        float spawnForward = attackSize.y - 0.3f;
        float spawnForwardMomentum = 0.1f;

        // Rotated in the direction the player is facing. The added 90 is to correct the rotation to the be the middle of where the camera is facing
        Quaternion attackRotation = Quaternion.Euler(playerCamera.transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, 0);
        // Spawns in front of the player and a bit extra in front based on how fast the player is moving
        Vector3 attackSpawnPosition = playerCamera.transform.position + playerCamera.transform.forward * spawnForward + playerRb.linearVelocity * spawnForwardMomentum;

        // Hits all enemues with a box in front of the player
        Collider[] hits = Physics.OverlapBox(attackSpawnPosition, attackSize, attackRotation, enemyLayer);
        foreach (Collider hit in hits)
        {
            if (hit.transform.TryGetComponent(out EnemyHealth health))
            {
                health.TakeDamage(damage);
                playerAudioScript.PlayFistsSound(true);
                return;
            }
        }
        playerAudioScript.PlayFistsSound(false);
    }
}
