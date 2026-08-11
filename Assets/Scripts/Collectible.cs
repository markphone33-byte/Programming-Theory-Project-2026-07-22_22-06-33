using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int prefabIndex;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerInventory.Instance.PickUpItem(itemName, prefabIndex);
            Destroy(gameObject);
        }
    }
}
