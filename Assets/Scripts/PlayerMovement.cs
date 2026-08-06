using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    [SerializeField] private float baseMoveSpeed = 45;
    [SerializeField] private float moveSpeedMultiplier = 1;
    [SerializeField] private float sprintBoost = 0.8f;
    private Camera playerCamera;
    private Rigidbody playerRb;
    private bool isSprinting = false;
    private bool isExhausted = false;
    private Slider sprintSlider;
    private float sprintFOVChange = 15;
    [SerializeField] private float sprintDuration = 3;
    [SerializeField] private float sprintRecoveryDuration = 3;

    // Initialize variables
    void Awake()
    {
        controls = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody>();
    }

    // Enables player input
    void OnEnable()
    {
        controls.Player.Enable();
    }

    // Disables player input
    void OnDisable()
    {
        controls.Player.Disable();
    }

    // Initialize references to other game objects
    void Start()
    {
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        sprintSlider = GameObject.FindWithTag("SprintSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        moveInput = controls.Player.Move.ReadValue<Vector2>();
    }

    // Movement is done in FixedUpdate to line up with the physics engine and prevent jittery movement
    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        // Calculates player's speed
        if (isSprinting)
        {
            moveSpeedMultiplier += sprintBoost;
        }
        float totalMoveSpeed = baseMoveSpeed * moveSpeedMultiplier;

        // Moves the player
        playerRb.AddForce(transform.forward * Time.fixedDeltaTime * moveInput.y * totalMoveSpeed, ForceMode.Impulse);
        playerRb.AddForce(transform.right * Time.fixedDeltaTime * moveInput.x * totalMoveSpeed, ForceMode.Impulse);

        moveSpeedMultiplier = 1; // Resets the move speed multiplier
    }

    private void Sprint()
    {
        // When player tries to sprint
        if (controls.Player.Sprint.IsPressed())
        {
            // If the player is starting sprint
            if (!isSprinting && sprintSlider.value > sprintSlider.minValue && !isExhausted)
            {
                isSprinting = true;
                StartCoroutine(FOVChange(sprintFOVChange, sprintFOVChange / 5)); // FOV increases by sprintFOVChange over 0.25 seconds
            }
        }
        // When the player is not trying to sprint
        else if (!controls.Player.Sprint.IsPressed())
        {
            // If the player just stopped sprinting
            if (isSprinting)
            {
                isSprinting = false;
                StartCoroutine(FOVChange(sprintFOVChange, -sprintFOVChange / 5)); // FOV reverts to normal over 0.25 seconds
            }
        }

        // Sprint bar refills while not spriting
        if (!isSprinting)
        {
            sprintSlider.value += Time.deltaTime / sprintRecoveryDuration; // Sprint bar takes sprintRecoveryDuration seconds to refill

            // When sprint bar fills to max, the player is no longer exhausted
            if (sprintSlider.value >= sprintSlider.maxValue)
            {
                isExhausted = false;
            }
        }
        // Sprint bar decreases while spriting
        else
        {
            sprintSlider.value -= Time.deltaTime / sprintDuration; // Sprint bar takes sprintDuration seconds to empty

            // When player runs out of sprint
            if (sprintSlider.value <= sprintSlider.minValue)
            {
                isSprinting = false;
                isExhausted = true;
                StartCoroutine(FOVChange(sprintFOVChange, -sprintFOVChange / 5)); // FOV reverts to normal over 0.25 seconds
            }
        }
    }

    // Every 0.05 seconds, the FOV is increased by stepValue until it is changed by the desired amount
    IEnumerator FOVChange(float amount, float stepValue)
    {
        while (amount > 0)
        {
            playerCamera.fieldOfView += stepValue;
            amount -= math.abs(stepValue);
            yield return new WaitForSeconds(0.05f);
        }
    }
}