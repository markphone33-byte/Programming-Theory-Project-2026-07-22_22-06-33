using UnityEngine;

public class FogOfWarEnter : MonoBehaviour
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
            controller.FogDisappear();
        }
    }
}
