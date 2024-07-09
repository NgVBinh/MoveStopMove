using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;


[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{

    public float moveSpeed;
    [SerializeField] private float radiusFindWay = 30f;
    [SerializeField] private float radiusSpawn = 20f;

    private LayerMask targetLayer;
    [SerializeField] private GameObject beTarget;
    public Collider col { get; private set; }

    public EnemyIdleState enemyIdleState { get; private set; }
    public EnemyMoveState enemyMoveState { get; private set; }
    public EnemyAttackState enemyAttackState { get; private set; }
    public EnemyDieState enemyDieState { get; private set; }

    public EnemyStateMachine stateMachine;

    public NavMeshAgent agent { get; private set; }

    public Action OnDeath;
    public bool enemyDead;

    private Player player;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        stateMachine = new EnemyStateMachine();
        enemyIdleState = new EnemyIdleState(this, stateMachine, "IsIdle");
        enemyMoveState = new EnemyMoveState(this, stateMachine, "IsMove");
        enemyAttackState = new EnemyAttackState(this, stateMachine, "IsAttack");
        enemyDieState = new EnemyDieState(this, stateMachine, "IsDead");
    }

    protected override void OnEnable()
    {
        //OnDeath += SetupDie;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(enemyMoveState);
        targetLayer = LayerMask.GetMask("Enemy", "Player");


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public void InitializeEnemy(string weaponName, Material bodyMat, Material pantMat, int level, Player player)
    {
        InitialWeapon(weaponName);
        body.material = bodyMat;
        pant.material = pantMat;
        this.level = level;
        this.player = player;

        SpawnOnNavMesh();
    }

    public override void ChangeIdleState()
    {
        base.ChangeIdleState();
        stateMachine.ChangeState(enemyIdleState);
    }

    public void EnemyIdle()
    {
        agent.velocity = Vector3.zero;
        agent.speed = 0;
        agent.ResetPath();

    }

    public void SpawnOnNavMesh()
    {
        // don't spawn near players according to the spawn radius
        Vector3 randomPosSpawn = GetRandomPointOnNavMesh(radiusSpawn);
        if (CheckPlayerRange(randomPosSpawn))
        {
            SpawnOnNavMesh();
            return;
        }
        Debug.Log(Vector3.Distance(randomPosSpawn, player.transform.position));

        transform.position = randomPosSpawn;
    }

    public void SetRandomDestination()
    {

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetRandomPointOnNavMesh(radiusFindWay));
        }
    }

    public Vector3 GetRandomPointOnNavMesh(float radius)
    {
        NavMeshHit hit;
        Vector3 randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * radius;

        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // If a random point is not on NavMesh, try again.
        return GetRandomPointOnNavMesh(radius);
    }

    protected virtual bool CheckPlayerRange(Vector3 checkPos)
    {
        if (Vector3.Distance(checkPos, player.transform.position) < radiusSpawn)
        {
            return true;
        }

        return false;
    }

    public Transform GetClosestTargetInRange()
    {
        return GetTargetInRange(targetLayer);
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        stateMachine.ChangeState(enemyDieState);
    }
    public override void KillCharacter()
    {
        base.KillCharacter();

    }

    public void BeTargetted(bool display)
    {
        beTarget.SetActive(display);
    }

    public void RevivalEnemy()
    {
        enemyDead = false;
        this.tag = "Enemy";
        col.enabled = true;
        agent.ResetPath();
        stateMachine.ChangeState(enemyMoveState);
    }
}
