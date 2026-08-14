using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    [SerializeField] private GameObject[] itemPrefabs;

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public GameObject GetItemPrefab(string name)
    {
        foreach (GameObject itemPrefab in itemPrefabs)
        {
            if (itemPrefab.name.Equals(name))
            {
                return itemPrefab;
            }
        }
        Debug.Log(name + " could not be found in ItemPrefabs");
        return null;
    }

    public GameObject GetItemPrefab(int index)
    {
        if (index < itemPrefabs.Length && index >= 0)
        {
            return itemPrefabs[index];
        }
        Debug.Log("Item could not be found at index " + index + " in ItemPrefabs");
        return null;
    }

    public int GetEnergyCrystalIndex()
    {
        return 1;
    }
}
