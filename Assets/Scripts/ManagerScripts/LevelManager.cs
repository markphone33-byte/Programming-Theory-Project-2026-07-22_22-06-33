using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public int level { get; private set; }
    private GameObject player;
    private bool gameStart = true;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI crystalsNeededText;
    public int crystalsNeeded { get; private set; }
    private GameObject enemyParent;
    private GameObject itemParent;
    private Vector3 spawnPosition;

    void Awake()
    {
        Instance = this;
        level = 1;
        spawnPosition = new Vector3(0, 0, -5);
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
        if (gameStart)
        {
            StartLevel();
            gameStart = false;
        }
    }

    private void StartLevel()
    {
        crystalsNeeded = 3 + 2 * level;
        levelText.text = "Level: " + level;
        crystalsNeededText.text = "Crystals Needed: " + crystalsNeeded;

        SpawnManager.Instance.SpawnLevel(level);
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
            if (enemy.gameObject != enemyParent)
            {
                Destroy(enemy.gameObject);
            }
        }

        foreach (Transform item in itemParent.GetComponentInChildren<Transform>())
        {
            if (item.gameObject != enemyParent)
            {
                Destroy(item.gameObject);
            }
        }

        player.transform.position = spawnPosition;
    }
}
