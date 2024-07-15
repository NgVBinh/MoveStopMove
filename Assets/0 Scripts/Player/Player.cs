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
    public PlayerWinState winState { get; private set; }
    public PlayerDanceSkinState danceSkinState { get; private set; }

    [Header("movement")]
    public JoystickController joystick;
    public bool canMove;

    [Header("Main Camera")]
    [SerializeField] private CameraFollow camFollow;


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
        winState = new PlayerWinState(this, stateMachine, "IsWin");
        danceSkinState = new PlayerDanceSkinState(this, stateMachine, "IsDance");
    }


    protected override void OnEnable()
    {
        stateMachine.Initialize(idleState);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //stateMachine.ChangeState(idleState);
        canMove = true;
        targetLayer = LayerMask.GetMask("Enemy");
        //InitialWeapon(weaponCenter);
        Observer.AddObserver("play", PlayerPlayGame);
        Observer.AddObserver("PlayerWin", ChangePlayerWinState);

        //CreateWeaponInHand();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        DisplayTarget();
        //Debug.Log(stateMachine.currentState);

    }

    public void CreateWeaponInHand()
    {
        //equipController.Equipment(weaponEquipments[0]);
        GameObject weaponInHand = equipController.GetEquipped(EquipmentType.WEAPON);
        weaponScript = weaponInHand.GetComponent<WeaponController>();
        weaponScript.GetComponent<WeaponController>()?.SetWeaponOfCharacter(true);
        weaponName = equipController.weaponName;

    }

    public void Equipment(EquipmentSO equip)
    {
        equipController.Equipment(equip,this);
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

    public override void TakeDamage()
    {
        base.TakeDamage();

        stateMachine.ChangeState(dieState);
    }

    public override void IncreaseScaleCharacter(float percent)
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

    //temp
    [SerializeField] private List<EquipmentSO> weaponEquipments;

    private void PlayerPlayGame()
    {
        attackRangSprite.SetActive(true);
        levelOnHead.SetActive(true);
    }
    public override void ChangeIdleState()
    {
        stateMachine.ChangeState(idleState);
    }
    private void ChangePlayerWinState()
    {
        stateMachine.ChangeState(winState);
    }


    private void OnDestroy()
    {
        Observer.RemoveObserver("play", PlayerPlayGame);
        Observer.RemoveObserver("PlayerWin", ChangePlayerWinState);
    }

}
