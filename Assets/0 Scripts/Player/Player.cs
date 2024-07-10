using System;
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
    public JoystickController joystick;
    public bool canMove;
    public float moveSpeed;

    [Header("Main Camera")]
    [SerializeField] private CameraFollow camFollow;

    [Header("Weapon")]
    [SerializeField] private string weapon;

    [Header("Infor Character")]
    [SerializeField] private GameObject attackRangSprite;
    [SerializeField] private GameObject levelOnHead;

    private LayerMask targetLayer;

    public Transform enemyTargetted { get; private set; }

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
        InitialWeapon(weapon);

        Observer.AddObserver("play", PlayerPlayGame);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        DisplayTarget();
        //Debug.Log(stateMachine.currentState);

    }



    public void MoveHandle(Vector3 moveDir)
    {
        if(!canMove) return;
        rb.velocity = moveDir;// new Vector3(moveDir.x, 0, moveDir.z);
    }


    public Transform GetClosestEnemyInRange()
    {
        return GetTargetInRange(targetLayer);
    }

    public override void ChangeIdleState()
    {
        stateMachine.ChangeState(idleState);
    }

    public override void KillCharacter()
    {
        base.KillCharacter();

    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        stateMachine.ChangeState(dieState);
    }

    protected override void IncreaseScaleCharacter(float percent)
    {
        base.IncreaseScaleCharacter(percent);
        camFollow.SetOffset(percent);
    }

    public void DisplayTarget()
    {
        
        if (enemyTargetted!=GetClosestEnemyInRange())
        {
            if(enemyTargetted!=null) {
                enemyTargetted.GetComponent<Enemy>().BeTargetted(false);
            }
            enemyTargetted = GetClosestEnemyInRange();
        }
        // attack enemy
        if (enemyTargetted!=null)
        {
            GetClosestEnemyInRange().GetComponent<Enemy>().BeTargetted(true);
        }
    }

    public override void InitialWeapon(string weapon)
    {
        base.InitialWeapon(weapon);
    }

    private void OnDestroy()
    {
        Observer.RemoveObserver("play", PlayerPlayGame);
    }

    private void PlayerPlayGame()
    {
        attackRangSprite.SetActive(true);
        levelOnHead.SetActive(true);
            
    }
}
