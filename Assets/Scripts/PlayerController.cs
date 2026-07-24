using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    [SerializeField] private Vector2 lookInput; // Remove SerializeField later
    [SerializeField] private float moveSpeed; // Remove SerializeField later
    [SerializeField] private float lookSpeed; // Remove SerializeField later
    [SerializeField] private float maxPitch; // Remove SerializeField later
    [SerializeField] private float currentPitch; // Remove SerializeField later
    private Transform playerCamera;
    private Rigidbody playerRb;


    void Awake()
    {
        controls = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = GameObject.FindWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LookMovement();
    }

    private void Movement()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        // transform.Translate(Vector3.forward * Time.deltaTime * moveInput.y * moveSpeed);
        // transform.Translate(Vector3.right * Time.deltaTime * moveInput.x * moveSpeed);

        playerRb.AddForce(transform.forward * Time.deltaTime * moveInput.y * moveSpeed, ForceMode.Impulse);
        playerRb.AddForce(transform.right * Time.deltaTime * moveInput.x * moveSpeed, ForceMode.Impulse);

    }

    void LookMovement()
    {
        lookInput = controls.Player.Look.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, lookInput.x * lookSpeed, Space.World);
        playerCamera.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);
        currentPitch -= lookInput.y * lookSpeed / 2;
        currentPitch = Mathf.Clamp(currentPitch, -maxPitch, maxPitch);

    }
}
