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
    private LayerMask wallsAndEnemies;

    // Initalize Variables
    void Awake()
    {
        currentHealth = maxHealth;
        healthText = transform.Find("HealthText").GetComponent<TextMeshPro>();
        healthText.text = currentHealth.ToString();
        enemyAttackScript = GetComponent<EnemyAttack>();
        wallsAndEnemies = LayerMask.GetMask("Default", "Enemy");
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
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Fires a ray from the player to the enemy and stores the information in hit
        Vector3 direction = transform.position - playerCamera.transform.position;
        RaycastHit hit;
        Physics.Raycast(playerCamera.transform.position, direction.normalized, out hit, direction.magnitude, wallsAndEnemies);
        // If the ray hits an object other than the enemy then make the health text invisible
        Color color = healthText.color;
        if (hit.transform != transform)
        {
            color.a = 0;
        }
        // If there is no wall or object blocking line of sight then make the health text darker the farther away the player is
        else
        {
            color.a = 1;
            color.r = Mathf.Clamp01(0.9f - enemyAttackScript.DistanceToPlayer() / 40);
        }
        healthText.color = color;
    }

    void LateUpdate()
    {
        // Makes the health text always face the player
        healthText.transform.forward = playerCamera.transform.forward;
    }


}
