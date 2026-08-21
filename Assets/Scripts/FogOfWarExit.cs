using UnityEngine;

public class FogOfWarExit : MonoBehaviour
{
    private FogOfWarController controller;

    void Awake()
    {
        controller = GetComponentInParent<FogOfWarController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerMapIndicator"))
        {
            controller.PlayerEntersCollider();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerMapIndicator"))
        {
            controller.PlayerExitsCollider();
        }
    }
}
