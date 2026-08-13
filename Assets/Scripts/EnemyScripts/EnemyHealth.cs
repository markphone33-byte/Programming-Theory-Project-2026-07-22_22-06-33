using System.Collections;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    private TextMeshPro healthText;
    private GameObject playerCamera;
    private EnemyAttack enemyAttackScript;
    private LayerMask wallsAndEnemies;
    [SerializeField] private GameObject EnergyCrystal;

    // Initalize Variables
    void Awake()
    {
        currentHealth = maxHealth;
        healthText = transform.Find("HealthText").GetComponent<TextMeshPro>();
        healthText.text = currentHealth.ToString();
        enemyAttackScript = GetComponent<EnemyAttack>();
        wallsAndEnemies = LayerMask.GetMask("Wall", "Enemy");
    }

    // Initialize references to other game objects
    void Start()
    {
        playerCamera = GameObject.FindWithTag("MainCamera");
    }

    // Reduces health and dies when at or below 0 health
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthText.text = currentHealth.ToString();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Update()
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
            color.r = Mathf.Clamp01(0.9f - Mathf.Round(playerDistance / 3) / 10);
            healthText.color = color;
        }
    }

    void LateUpdate()
    {
        // Makes the health text always face the player
        healthText.transform.forward = playerCamera.transform.forward;
    }

    private void Die()
    {
        Destroy(gameObject);
        Vector3 crystalPosition = transform.position;
        crystalPosition.y = 1;
        Instantiate(EnergyCrystal, crystalPosition, EnergyCrystal.transform.rotation);
    }
}
