using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private InputSystem_Actions controls;
    [SerializeField] private ParticleSystem slashParticle;
    private GameObject playerCamera;
    private Rigidbody playerRb;
    [SerializeField] private float spawnForward = 2.5f;
    private LayerMask enemyLayer;
    [SerializeField] private Vector3 attackHalfExtents = new Vector3(2f, 3f, 0.7f);
    [SerializeField] private float spawnMomentum = 0.1f;
    [SerializeField] private float damage = 10f;
   [SerializeField] private float attackCooldown = 0.5f;
    private float attackAvailableTime;


    // Initialize variables
    void Awake()
    {
        controls = new InputSystem_Actions();
        playerCamera = GameObject.FindWithTag("MainCamera");
        playerRb = GetComponent<Rigidbody>();
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    //Enables player input
    void OnEnable()
    {
        controls.Player.Enable();
    }
    // Disables player input
    void OnDisable()
    {
        controls.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.Player.Attack.triggered && Time.time >= attackAvailableTime)
        {
            // Rotated in the direction the player is facing. The added 90 is to correct the rotation to the be the middle of where the camera is facing
            Quaternion particleRotation = Quaternion.Euler(playerCamera.transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, 0);
            // Spawns in front of the player and a bit extra in front based on how fast the player is moving
            Vector3 particleSpawnPosition = playerCamera.transform.position + playerCamera.transform.forward * spawnForward + playerRb.linearVelocity * spawnMomentum;

            Instantiate(slashParticle, particleSpawnPosition, particleRotation);

            // Hits all enemues with a box in front of the player
            Collider[] hits = Physics.OverlapBox(particleSpawnPosition, attackHalfExtents, particleRotation, enemyLayer);
            foreach (Collider hit in hits)
            {
                hit.transform.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            }

            // Cooldown for attacking
            attackAvailableTime = Time.time + attackCooldown;
        }
    }

    // Makes a red box in the scene view showing the hitbox of the attack
    private void OnDrawGizmosSelected()
    {
        // Don't try to draw if the camera hasn't been found yet
        if (playerCamera == null)
            return;

        Quaternion rotation = Quaternion.Euler(
            playerCamera.transform.rotation.eulerAngles.x + 90,
            transform.rotation.eulerAngles.y,
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
