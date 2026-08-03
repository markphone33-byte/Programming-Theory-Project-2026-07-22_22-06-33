using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private float maxHealth = 100f;
    private float currentHealth;
    private Slider healthSlider;
    private bool healthBuffer = true;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        healthSlider = GameObject.FindWithTag("HealthSlider").GetComponent<Slider>();
        healthSlider.value = currentHealth / maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0 && currentHealth >= -15 && healthBuffer == true)
        {
            healthBuffer = false;
            currentHealth = 1;
        }
        else if(currentHealth <= 0)
        {
            currentHealth = 0;
            healthSlider.gameObject.transform.Find("Fill Area").gameObject.SetActive(false); // Makes health bar fully empty when the player dies
        }
        healthSlider.value = currentHealth / maxHealth;
    }
}
