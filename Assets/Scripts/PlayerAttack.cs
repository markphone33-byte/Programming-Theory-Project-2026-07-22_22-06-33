using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private ParticleSystem slashParticle;
    private GameObject playerCamera;
    private Rigidbody playerRb;
    public float tempOffset;
    public float spawnForward;
    private LayerMask enemyLayer;
    public Vector3 attackHalfExtents;
    public float spawnMomentum;


    void Awake()
    {
        playerCamera = GameObject.FindWithTag("MainCamera");
        playerRb = GetComponent<Rigidbody>();
        enemyLayer = LayerMask.GetMask("Enemy");
        attackHalfExtents = new Vector3(1f, 2f, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Quaternion particleRotation = Quaternion.Euler(playerCamera.transform.rotation.eulerAngles.x + tempOffset, transform.rotation.eulerAngles.y, 0);
            Vector3 particleSpawnPosition = playerCamera.transform.position + playerCamera.transform.forward * spawnForward + playerRb.linearVelocity * spawnMomentum;
            Instantiate(slashParticle, particleSpawnPosition, particleRotation);

            Collider[] hits = Physics.OverlapBox(particleSpawnPosition, attackHalfExtents, particleRotation, enemyLayer);
            Debug.Log(hits.Length);
            foreach (Collider hit in hits)
            {
                Debug.Log(hit.gameObject.name);
                hit.transform.Find("HealthText")?.GetComponent<EnemyHealth>()?.TakeDamage(10);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Don't try to draw if the camera hasn't been found yet
        if (playerCamera == null)
            return;

        Quaternion rotation = Quaternion.Euler(
            playerCamera.transform.rotation.eulerAngles.x + tempOffset,
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
