using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    private TextMeshPro healthText;
    private GameObject playerCamera;
    private EnemyAttack enemyAttackScript;
    private EnemyAudio enemyAudioScript;
    private LayerMask wallsAndEnemies;
    [SerializeField] private GameObject EnergyCrystal;
    private Renderer[] renderers;
    private Color[] originalColors;
    [SerializeField] private GameObject enemyModel;
    [SerializeField] private float flashDuration = 0.15f;
    private Coroutine tookDamageFlash = null;
    private EnemyMovement enemyMovementScript;
    private float playerLightDistance;

    // Initalize Variables
    void Awake()
    {
        currentHealth = maxHealth;
        healthText = transform.Find("HealthText").GetComponent<TextMeshPro>();
        healthText.text = currentHealth.ToString();
        enemyAttackScript = GetComponent<EnemyAttack>();
        enemyAudioScript = GetComponent<EnemyAudio>();
        enemyMovementScript = GetComponent<EnemyMovement>();
        wallsAndEnemies = LayerMask.GetMask("Wall", "Enemy");
        renderers = enemyModel.GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    // Initialize references to other game objects
    void Start()
    {
        playerCamera = GameObject.FindWithTag("MainCamera");
        playerLightDistance = playerCamera.GetComponentInChildren<Light>().range;
    }

    // Reduces health and dies when at or below 0 health
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthText.text = currentHealth.ToString();

        enemyAudioScript.PlayGruntSoud();

        enemyMovementScript.GoToPosition(playerCamera.transform.position);

        if (tookDamageFlash == null)
        {
            tookDamageFlash = StartCoroutine(DamageFlashRoutine(flashDuration));
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        // Makes health text invisible if player can't see the enemy and harder to see the farther away it is
        HealthTextVisibility();
    }

    void LateUpdate()
    {
        // Makes the health text always face the player
        healthText.transform.forward = playerCamera.transform.forward;
    }

    private void HealthTextVisibility()
    {
        // Fires a ray from the player to the enemy and stores the information in hit
        BoxCollider enemyCollider = GetComponent<BoxCollider>();
        Vector3 direction = enemyCollider.bounds.center - playerCamera.transform.position;
        RaycastHit hit;
        bool didHit = Physics.Raycast(playerCamera.transform.position, direction.normalized, out hit, direction.magnitude, wallsAndEnemies);
        // If the ray hits an object other than the enemy then make the health text invisible
        Color color = healthText.color;
        if (hit.transform != transform)
        {
            if (healthText.color.a != 0)
            {
                color.a = 0;
                healthText.color = color;
            }
        }
        // If there is no wall or object blocking line of sight then make the health text darker the farther away the player is
        else if (didHit)
        {
            color.a = 1;
            float playerDistance = enemyAttackScript.EnemyDistanceToPlayer();
            color.r = Mathf.Clamp01(0.7f - playerDistance / playerLightDistance);
            healthText.color = color;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        Vector3 crystalPosition = transform.position;
        crystalPosition.y = 1;
        Instantiate(EnergyCrystal, crystalPosition, EnergyCrystal.transform.rotation);
    }

    private IEnumerator DamageFlashRoutine(float duration)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = Color.red;
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }

        tookDamageFlash = null;
    }
}
