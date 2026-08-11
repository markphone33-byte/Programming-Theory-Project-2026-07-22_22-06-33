using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    private InputSystem_Actions controls;
    // [SerializeField] private ParticleSystem slashParticle;
    // private GameObject playerCamera;
    // private Rigidbody playerRb;
    // private LayerMask enemyLayer;
    [SerializeField] private float itemUsageCooldown = 0.5f;
    private float itemUsageAvailableTime;

    // Initialize variables
    void Awake()
    {
        controls = new InputSystem_Actions();
        // playerCamera = GameObject.FindWithTag("MainCamera");
        // playerRb = GetComponent<Rigidbody>();
        // enemyLayer = LayerMask.GetMask("Enemy");
    }

    //Enables player input
    void OnEnable()
    {
        controls.Player.Attack.Enable();
    }
    // Disables player input
    void OnDisable()
    {
        controls.Player.Attack.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.Player.Attack.triggered && Time.time >= itemUsageAvailableTime)
        {
            GameObject selectedItemPrefab = PlayerInventory.Instance.GetSelectedItemPrefab();
            selectedItemPrefab.GetComponent<UseItem>().Use();

            // Cooldown for using an item
            itemUsageAvailableTime = Time.time + itemUsageCooldown;
        }
    }
}
