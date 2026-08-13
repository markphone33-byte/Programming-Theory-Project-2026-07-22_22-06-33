using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Vector3 spawnCenter;
    [SerializeField] private float spawnRangeX;
    [SerializeField] private float spawnRangeZ;
    private GameObject player;
    [SerializeField] private GameObject energyCrystal;
    public int startingCount;
    private float minDistanceFromPlayer;

    void Awake()
    {
        minDistanceFromPlayer = 20;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        SpawnObject(energyCrystal, startingCount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnObject(GameObject obj, int count)
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
                        Instantiate(obj, spawnPos, obj.transform.rotation);
                        i++;
                    }
                }
            }
            attempts++;
        }
    }
}
