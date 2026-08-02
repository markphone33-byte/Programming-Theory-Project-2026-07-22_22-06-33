using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    [SerializeField] private float baseMoveSpeed = 45;
    [SerializeField] private float moveSpeedMultiplier = 1;
    [SerializeField] private float sprintBoost = 0.8f;
    private Transform playerCamera;
    private Rigidbody playerRb;


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
        playerCamera = GameObject.FindWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Movement();
    }

    private void Movement()
    {
        float totalMoveSpeed = baseMoveSpeed * moveSpeedMultiplier;

        moveInput = controls.Player.Move.ReadValue<Vector2>();
        playerRb.AddForce(transform.forward * Time.deltaTime * moveInput.y * totalMoveSpeed, ForceMode.Impulse);
        playerRb.AddForce(transform.right * Time.deltaTime * moveInput.x * totalMoveSpeed, ForceMode.Impulse);

    }

    // Sprint makes the player move 80% faster
    private void Sprint()
    {
        if (controls.Player.Sprint.WasPressedThisFrame())
        {
            moveSpeedMultiplier += sprintBoost;
            StartCoroutine(FOVChange(15, 3f)); // Increases FOV by 15
        }

        if (controls.Player.Sprint.WasReleasedThisFrame())
        {
            moveSpeedMultiplier -= sprintBoost;
            StartCoroutine(FOVChange(15, -3f)); // Reverts FOV to normal
        }
    }

    // Every 0.05 seconds, the FOV is increased by stepValue until it is changed by the desired amount
    IEnumerator FOVChange(float amount, float stepValue)
    {
        while (amount > 0)
        {
            yield return new WaitForSeconds(0.05f);
            playerCamera.gameObject.GetComponent<Camera>().fieldOfView += stepValue;
            amount -= math.abs(stepValue);
        }
    }
}
