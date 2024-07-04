using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{

    public float moveSpeed;
    private LayerMask targetLayer;
    [SerializeField] private GameObject beTarget;
    public Collider col;

    public EnemyIdleState enemyIdleState { get; private set; }
    public EnemyMoveState enemyMoveState { get; private set; }
    public EnemyAttackState enemyAttackState { get; private set; }
    public EnemyDieState enemyDieState { get; private set; }

    public EnemyStateMachine stateMachine;

    public NavMeshAgent agent { get; private set; }

    public Action OnDeath;
    public bool enemyDead;
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

    //public bool CheckPlayerInRange()
    //{
    //    if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
    //    {
    //        return true;
    //    }

    //    return false;
    //}


    protected override void ChangeIdleState()
    {
        base.ChangeIdleState();
        stateMachine.ChangeState(enemyIdleState);
    }

    public void EnemyIdle()
    {
        agent.SetDestination(transform.position);// enemy dung im
        agent.ResetPath();
        agent.isStopped = true;

    }

    public void SetRandomDestination()
    {
        // Debug.Log(enemy.agent.pathPending+"   "+ enemy.agent.remainingDistance);
        agent.isStopped = false;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetRandomPointOnNavMesh());
        }
    }

    public Vector3 GetRandomPointOnNavMesh()
    {
        NavMeshHit hit;
        Vector3 randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * 30f;

        if (NavMesh.SamplePosition(randomPoint, out hit, 30f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // If a random point is not on NavMesh, try again.
        return GetRandomPointOnNavMesh();
    }

    public Transform GetClosestTargetInRange()
    {
        return GetTargetInRange(targetLayer);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Weapon"))
    //    {
    //        if (enemyDead) return;
    //        stateMachine.ChangeState(enemyDieState);

    //    }
    //}
    public override void TakeDamage()
    {
        base.TakeDamage();
        //if (enemyDead) return;
        stateMachine.ChangeState(enemyDieState);
    }
    public override void KillCharacter()
    {
        base.KillCharacter();

    }

    public void BeTargetted()
    {
        beTarget.SetActive(true);
    }
}
