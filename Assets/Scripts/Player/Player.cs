using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerDieState dieState { get; private set; }

    [Header("movement")]
    public Joystick joystick;
    public bool canMove;
    public float moveSpeed;

    private LayerMask targetLayer;
    [SerializeField] private CameraFollow camFollow;

    //[SerializeField] private GameObject characterWeapon;
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this,stateMachine,"IsIdle");
        moveState = new PlayerMoveState(this,stateMachine,"IsMove");
        attackState = new PlayerAttackState(this, stateMachine, "IsAttack");
        dieState = new PlayerDieState(this, stateMachine, "IsDead");

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        canMove = true;
        targetLayer = LayerMask.GetMask("Enemy");

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

  

    public void MoveHandle(Vector3 moveDir)
    {
        if(!canMove) return;
        rb.velocity = new Vector3(moveDir.x, 0, moveDir.z);
    }


    public Transform GetClosestEnemyInRange()
    {
        return GetTargetInRange(targetLayer);
    }

    protected override void ChangeIdleState()
    {
        stateMachine.ChangeState(idleState);
    }

    public override void KillCharacter()
    {
        base.KillCharacter();
        //this.transform.localScale *= 1.1f;

    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        Debug.Log("player Dead");
        //stateMachine.ChangeState(dieState);
    }

    protected override void IncreaseScaleCharacter(float percent)
    {
        base.IncreaseScaleCharacter(percent);
        camFollow.SetOffset(percent);
    }

}
