using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    private TextMeshPro healthText;
    private GameObject playerCamera;
    private EnemyAttack enemyAttackScript;
    private Transform enemy;
    [SerializeField] private LayerMask visibilityMask;

    // Initalize Variables
    void Awake()
    {
        currentHealth = maxHealth;
        healthText = GetComponent<TextMeshPro>();
        healthText.text = currentHealth.ToString();
        enemyAttackScript = gameObject.GetComponentInParent<EnemyAttack>();
        enemy = transform.parent;
    }

    // Initialize references to other game objects
    void Start()
    {
        playerCamera = GameObject.FindWithTag("MainCamera");
    }

    // Reduces health and dies when at or below 0 health
    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy Damaged");
        currentHealth -= damage;
        healthText.text = currentHealth.ToString();
        if(currentHealth <= 0)
        {
            Destroy(enemy.gameObject);
        }
    }

    void Update()
    {
        // Fires a ray from the player to the enemy and stores the information in hit
        Vector3 direction = enemy.position - playerCamera.transform.position;
        RaycastHit hit;
        Physics.Raycast(playerCamera.transform.position, direction.normalized, out hit, direction.magnitude, visibilityMask);
        // If the ray hits an object other than the enemy then make the health text invisible
        Color color = healthText.color;
        if (hit.transform != enemy)
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

    // Makes the health text always face the player
    void LateUpdate()
    {
        transform.forward = playerCamera.transform.forward;
    }
}
