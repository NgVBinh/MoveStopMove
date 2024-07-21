using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{


    public Player player;
    [SerializeField] private NavMeshAgent agent;
    public SkinnedMeshRenderer bodyColor;

    [SerializeField] private float radiusSpawn;
    [SerializeField] private GameObject beTarget;

    [SerializeField] private ParticleSystem dieEfect;


    private void Awake()
    {
        player = ZCGameManager.instance.player;
        
    }
    private void OnEnable()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        dieEfect.GetComponent<Renderer>().material = bodyColor.material;

    }
    private void Update()
    {
        agent.SetDestination(player.transform.position);
    }

    public void SetupZombie(Material bodyMat)
    {
        bodyColor.material = bodyMat;
    }

    public void SpawnOnNavMesh()
    {
        // don't spawn near players according to the spawn radius
        Vector3 randomPosSpawn = GetRandomPointOnNavMesh(player.transform.position, radiusSpawn);
        if (Vector3.Distance(randomPosSpawn, player.transform.position) < player.attackRange)
        {
            SpawnOnNavMesh();
            return;
        }
        // Debug.Log(Vector3.Distance(randomPosSpawn, player.transform.position));

        transform.position = randomPosSpawn;
    }

    public Vector3 GetRandomPointOnNavMesh(Vector3 centerPos, float radius)
    {
        NavMeshHit hit;
        Vector3 randomPoint = centerPos + Random.insideUnitSphere * radius;

        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // If a random point is not on NavMesh, try again.
        return GetRandomPointOnNavMesh(centerPos, radius);
    }

    public void BeTargetted(bool display)
    {
        beTarget.SetActive(display);
    }

    public void TakeDamege()
    {
        agent.speed = 0;
        dieEfect.Play();
        Destroy(gameObject,0.5f);
    }
}
