using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private GameObject item;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerInventory.Instance.PickUpItem(itemName, item);
            Destroy(gameObject);
        }
    }
}
