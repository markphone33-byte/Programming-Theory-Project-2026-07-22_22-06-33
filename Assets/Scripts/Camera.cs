using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    void Awake()
    {
        offset = new Vector3(0, 1.5f, 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
