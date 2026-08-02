using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private InputSystem_Actions controls;
    [SerializeField] private Vector2 lookInput; // Remove SerializeField later
    [SerializeField] private float lookSpeed = 2; // Remove SerializeField later
    [SerializeField] private float maxPitch = 80; // Remove SerializeField later
    private float currentPitch = 0f; // Remove SerializeField later
    private Transform playerCamera;

    // Initialize variables
    void Awake()
    {
        controls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    // Locks cursor, hides cursor, and initializes references to other game objects
    void Start()
    {
        playerCamera = GameObject.FindWithTag("MainCamera").transform;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        LookMovement();
    }

    private void LookMovement()
    {
        lookInput = controls.Player.Look.ReadValue<Vector2>(); // Gets horizontal and vertical mouse movement

        transform.Rotate(Vector3.up, lookInput.x * lookSpeed, Space.World); // Rotates the player and camera left and right based on horizontal mouse movement
        playerCamera.localRotation = Quaternion.Euler(currentPitch, 0f, 0f); // Rotates the camera up and down based on vertical mouse movement

        currentPitch -= lookInput.y * lookSpeed / 2; // Tracks the current pitch (up and down rotation) of the camera
        currentPitch = Mathf.Clamp(currentPitch, -maxPitch, maxPitch); // Clamps the current pitch to prevent the camera from rotating too far up or down
    }
}
