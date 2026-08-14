using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerAttack : MonoBehaviour
{
    [SerializeField] private ParticleSystem slashParticle;
    [SerializeField] private Vector3 attackHalfExtents = new Vector3(2f, 1.5f, 0.7f);
    [SerializeField] private float damage = 10f;
    [SerializeField] private float spawnForward = 1.3f;
    [SerializeField] private float spawnMomentum = 0.1f;
    [SerializeField] private float particleLifetime = 0.1f;
    [SerializeField] private float particleSpeed = 8f;
    [SerializeField] private float particleSizeXMultiplier = 4f;
    [SerializeField] private float particleSizeYMultiplier = 1f;
    [SerializeField] private float particleSizeZMultiplier = 1f;

    void Update()
    {
        if(Keyboard.current.tKey.wasPressedThisFrame)
        {
            Attack();
        }
    }
    private void Attack()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject playerCamera = GameObject.FindWithTag("MainCamera");
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");


        // Rotated in the direction the player is facing. The added 90 is to correct the rotation to the be the middle of where the camera is facing
        Quaternion particleRotation = Quaternion.Euler(playerCamera.transform.rotation.eulerAngles.x + 90, player.transform.rotation.eulerAngles.y, 0);
        // Spawns in front of the player and a bit extra in front based on how fast the player is moving
        Vector3 particleSpawnPosition = playerCamera.transform.position + playerCamera.transform.forward * spawnForward + playerRb.linearVelocity * spawnMomentum;

        ParticleSystem attackParticle = Instantiate(slashParticle, particleSpawnPosition, particleRotation);
        ParticleSystem.MainModule particleMain = attackParticle.main;
        particleMain.startLifetime = particleLifetime;
        particleMain.startSpeed = particleSpeed;
        particleMain.startSizeXMultiplier = particleSizeXMultiplier;
        particleMain.startSizeYMultiplier = particleSizeYMultiplier;
        particleMain.startSizeZMultiplier = particleSizeZMultiplier;
        attackParticle.Play();

        // Hits all enemues with a box in front of the player
        Collider[] hits = Physics.OverlapBox(particleSpawnPosition, attackHalfExtents, particleRotation, enemyLayer);
        foreach (Collider hit in hits)
        {
            hit.transform.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }

    // Makes a red box in the scene view showing the hitbox of the attack
    private void OnDrawGizmos()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject playerCamera = GameObject.FindWithTag("MainCamera");
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        // Don't try to draw if the camera hasn't been found yet
        if (playerCamera == null)
            return;

        Quaternion rotation = Quaternion.Euler(
            playerCamera.transform.rotation.eulerAngles.x + 90,
            player.transform.rotation.eulerAngles.y,
            0);

        Vector3 center =
            playerCamera.transform.position +
            playerCamera.transform.forward * spawnForward +
            playerRb.linearVelocity * spawnMomentum;

        Gizmos.color = Color.red;

        // // Save the current Gizmos transform
        Matrix4x4 oldMatrix = Gizmos.matrix;

        // // Make the Gizmo use the same position and rotation as the OverlapBox
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);

        // Draw the box. DrawWireCube expects FULL size, not half extents.
        Gizmos.DrawWireCube(Vector3.zero, attackHalfExtents * 2);
        Gizmos.DrawWireCube(center, attackHalfExtents * 2);


        // Restore the previous matrix
        Gizmos.matrix = oldMatrix;
    }
}
