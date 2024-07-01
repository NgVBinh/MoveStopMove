using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private PlayerManager player;

    public float attackCooldown;
    public float rangeAttack;
    public EnemyIdleState enemyIdleState {  get; private set; }
    public EnemyMoveState enemyMoveState {  get; private set; }
    public EnemyAttackState enemyAttackState {  get; private set; }

    public EnemyStateMachine stateMachine;
    public Animator animator { get; private set; }

    public NavMeshAgent agent { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyStateMachine();
        enemyIdleState = new EnemyIdleState(this, stateMachine, "IsIdle");
        enemyMoveState = new EnemyMoveState(this, stateMachine, "IsMove");
        enemyAttackState = new EnemyAttackState(this, stateMachine, "IsAttack");
    }
    // Start is called before the first frame update
    void Start()
    {
        stateMachine.Initialize(enemyMoveState);
        player = PlayerManager.instance;

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
    }

    public bool CheckPlayerInRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < rangeAttack)
        {
            return true;
        }

        return false;
    }
}
