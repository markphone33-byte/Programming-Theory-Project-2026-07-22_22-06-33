using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Vector3 spawnCenter;
    [SerializeField] private float spawnRangeX;
    [SerializeField] private float spawnRangeZ;
    private GameObject player;
    private float minDistanceFromPlayer;
    public static SpawnManager Instance { get; private set; }
    [SerializeField] private GameObject[] enemyPrefabs;
    private GameObject enemyParent;
    private GameObject itemParent;

    void Awake()
    {
        minDistanceFromPlayer = 20;
        Instance = this;
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

    }

    public void SpawnLevel(int level)
    {
        int enemiesToSpawn = 3 + level * 2;
        int crystalsToSpawn = 7 + level * 2;

        SpawnObject(enemyPrefabs[0], enemiesToSpawn, enemyParent);
        GameObject energyCrystal = ItemManager.Instance.GetItemPrefab("EnergyCrystal");
        if (energyCrystal != null)
        {
            SpawnObject(energyCrystal, crystalsToSpawn, itemParent);
        }
    }

    private void SpawnObject(GameObject obj, int count, GameObject parent)
    {
        int i = 0;
        int attempts = 0;
        while (i < count && attempts < 1000)
        {
            Vector3 randomPos = new Vector3
            (
                Random.Range(spawnCenter.x - spawnRangeX, spawnCenter.x + spawnRangeX),
                obj.transform.position.y,
                Random.Range(spawnCenter.z - spawnRangeZ, spawnCenter.z + spawnRangeZ)
            );
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 5, NavMesh.AllAreas))
            {
                if (Vector3.Distance(player.transform.position, hit.position) > minDistanceFromPlayer)
                {
                    NavMeshQueryFilter filter = new NavMeshQueryFilter
                    {
                        areaMask = NavMesh.AllAreas,
                        agentTypeID = 0
                    };
                    Vector3 spawnPos = hit.position + Vector3.up;
                    NavMeshPath path = new NavMeshPath();
                    NavMesh.CalculatePath(player.transform.position, spawnPos, filter, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        Instantiate(obj, spawnPos, obj.transform.rotation, parent.transform);
                        i++;
                    }
                }
            }
            attempts++;
        }
    }
}
