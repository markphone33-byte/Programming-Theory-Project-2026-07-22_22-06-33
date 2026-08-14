using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int level {get; private set;}
    [SerializeField] private GameObject[] enemyPrefabs;
    private GameObject enemyParent;
    private GameObject itemParent;
    private GameObject player;
    private bool gameStart = true;

    void Awake()
    {
        level = 1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        itemParent = GameObject.FindWithTag("ItemParent");
        enemyParent = GameObject.FindWithTag("EnemyParent");
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStart)
        {
            StartLevel();
            gameStart = false;
        }
    }

    private void StartLevel()
    {
        int enemiesToSpawn = 3 + level * 2;
        int crystalsToSpawn = 7 + level * 2;

        SpawnManager.Instance.SpawnObject(enemyPrefabs[0], enemiesToSpawn, enemyParent);
        GameObject energyCrystal = ItemManager.Instance.GetItemPrefab("EnergyCrystal");
        if (energyCrystal != null)
        {
            SpawnManager.Instance.SpawnObject(energyCrystal, crystalsToSpawn, itemParent);
        }
    }

    public void NextLevel()
    {
        level++;
        ResetMap();
        StartLevel();
    }

    private void ResetMap()
    {
        foreach (Transform enemy in enemyParent.GetComponentInChildren<Transform>())
        {
            if(enemy.gameObject != enemyParent)
            {
                Destroy(enemy.gameObject);
            }
        }

        foreach (Transform item in itemParent.GetComponentInChildren<Transform>())
        {
            if(item.gameObject != enemyParent)
            {
                Destroy(item.gameObject);
            }
        }

        player.transform.position = Vector3.zero;
    }
}
