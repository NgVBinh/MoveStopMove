using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{

    [SerializeField] private float radiusFindWay;
    [SerializeField] private float radiusSpawn;

    [Header("Enemy infor")]
    [SerializeField] private GameObject beTarget;
    [SerializeField] private GameObject levelOnHead;


    [Header("Equipment")]
    [SerializeField] private List<EquipmentSO> weaponEquipments = new List<EquipmentSO>();
    [SerializeField] private List<EquipmentSO> hairEquipments = new List<EquipmentSO>();
    [SerializeField] private List<EquipmentSO> shieldEquipments = new List<EquipmentSO>();

    private LayerMask targetLayer;
    public NavMeshAgent agent { get; private set; }
    public Action OnDeath;
    //public bool enemyDead;

    private Player player;
    public Collider col { get; private set; }

    #region enemy state
    public EnemyPrepareState enemyPrepareState { get; private set; }
    public EnemyIdleState enemyIdleState { get; private set; }
    public EnemyMoveState enemyMoveState { get; private set; }
    public EnemyAttackState enemyAttackState { get; private set; }
    public EnemyDieState enemyDieState { get; private set; }

    public EnemyStateMachine stateMachine;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        stateMachine = new EnemyStateMachine();
        enemyPrepareState = new EnemyPrepareState(this, stateMachine, "IsIdle");
        enemyIdleState = new EnemyIdleState(this, stateMachine, "IsIdle");
        enemyMoveState = new EnemyMoveState(this, stateMachine, "IsMove");
        enemyAttackState = new EnemyAttackState(this, stateMachine, "IsAttack");
        enemyDieState = new EnemyDieState(this, stateMachine, "IsDead");
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(enemyPrepareState);
        targetLayer = LayerMask.GetMask("Enemy", "Player");

        Observer.AddObserver("play", EnemyPlay);

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public void InitializeEnemy(Material bodyMat, Material pantMat, int level,string name, Player player)
    {
        characterName = name;
        //InitialWeapon(weaponName);
        body.material = bodyMat;
        pant.material = pantMat;
        this.level = level;
        this.player = player;

        SpawnOnNavMesh();
        InitializeEquipment();
        
    }

    private void InitializeEquipment()
    {
        //equip
        equipController = GetComponentInChildren<EquipmentCharacter>();
        equipController.Equipment(weaponEquipments[UnityEngine.Random.Range(0,weaponEquipments.Count)]);
        equipController.Equipment(hairEquipments[UnityEngine.Random.Range(0, hairEquipments.Count)]);
        equipController.Equipment(shieldEquipments[UnityEngine.Random.Range(0, shieldEquipments.Count)]);

        GameObject weaponInHand = equipController.GetEquipped(EquipmentType.WEAPON);
        weaponScript =weaponInHand.GetComponent<WeaponController>();
        weaponScript.GetComponent<WeaponController>()?.SetWeaponOfCharacter(true);
        weaponName = equipController.weaponName;
    }


    public void SpawnOnNavMesh()
    {
        // don't spawn near players according to the spawn radius
        Vector3 randomPosSpawn = GetRandomPointOnNavMesh(player.transform.position,radiusSpawn);
        if (Vector3.Distance(randomPosSpawn, player.transform.position) < player.attackRange + 3)
        {
            SpawnOnNavMesh();
            return;
        }
       // Debug.Log(Vector3.Distance(randomPosSpawn, player.transform.position));

        transform.position = randomPosSpawn;
    }

    public void SetRandomDestination()
    {

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetRandomPointOnNavMesh(transform.position,radiusFindWay));
        }
    }

    public Vector3 GetRandomPointOnNavMesh(Vector3 centerPos,float radius)
    {
        NavMeshHit hit;
        Vector3 randomPoint = centerPos + UnityEngine.Random.insideUnitSphere * radius;

        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // If a random point is not on NavMesh, try again.
        return GetRandomPointOnNavMesh(centerPos,radius);
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

    public void BeTargetted(bool display)
    {
        beTarget.SetActive(display);
    }
    #region controll enemy state
    public void RevivalEnemy()
    {
        //enemyDead = false;
        this.tag = "Enemy";
        col.enabled = true;
        agent.ResetPath();
        stateMachine.ChangeState(enemyMoveState);
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

    // change prepare state when play game
    private void EnemyPlay()
    {
        levelOnHead.SetActive(true);

        if(UnityEngine.Random.Range(0, 2) == 0)
        {
            stateMachine.ChangeState(enemyIdleState);
        }
        else
        {
            stateMachine.ChangeState(enemyMoveState);
        }
    }
    #endregion
    private void OnDestroy()
    {
        Observer.RemoveObserver("play", EnemyPlay);
    }

}
