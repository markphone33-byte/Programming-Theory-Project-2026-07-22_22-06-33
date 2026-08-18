using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float forwardOffsetMultiplier;
    [SerializeField] float cameraRadius = 2;

    void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit hit;
        Vector3 normalPosition = player.transform.position + offset;
        Vector3 forwardOffsetPosition = normalPosition + transform.forward * forwardOffsetMultiplier;
        // Vector3 direction = forwardOffsetPosition - normalPosition;
        // if (Physics.SphereCast(normalPosition, cameraRadius, direction.normalized, out hit, direction.magnitude, LayerMask.GetMask("Wall", "Enemy")))
        // {
        //     transform.position = hit.point - direction.normalized * cameraRadius;
        //     Debug.Log("Hit");
        // }
        // else
        // {
        //     transform.position = forwardOffsetPosition;
        // }
        transform.position = forwardOffsetPosition;

    }

}
