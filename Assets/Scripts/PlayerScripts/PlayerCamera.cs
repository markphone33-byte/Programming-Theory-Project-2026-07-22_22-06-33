using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float tempValue;

    void Awake()
    {
        offset = new Vector3(0, 1f, 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + transform.forward * tempValue + offset;
    }
    
}
