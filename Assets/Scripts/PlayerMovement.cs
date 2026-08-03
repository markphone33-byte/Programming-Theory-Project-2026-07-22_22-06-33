using System.Collections;
using Unity.Mathematics;
using UnityEngine;

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
        // If the sprint button is pressed the move speed multiplier is increased and the FOV is increased
        if (controls.Player.Sprint.IsPressed() && !isSprinting)
        {
            isSprinting = true;
            StartCoroutine(FOVChange(15, 3));
        }
        // If the sprint button is released the FOV is decreased and Movement() automatically resets the speed multiplier
        else if (!controls.Player.Sprint.IsPressed() && isSprinting)
        {
            isSprinting = false;
            StartCoroutine(FOVChange(15, -3));
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
