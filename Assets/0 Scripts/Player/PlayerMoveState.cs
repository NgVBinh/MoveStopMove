using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
    }

    private Vector3 moveDir;


    public override void Enter()
    {
        base.Enter();
        //moveDir = new Vector3(player.joystick.Direction.x, 0,player.joystick.Direction.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (moveDir == Vector3.zero) stateMachine.ChangeState(player.idleState);
        moveDir = new Vector3(player.joystick.Direction.x, 0, player.joystick.Direction.y);
        player.MoveHandle(moveDir.normalized * Time.deltaTime * player.moveSpeed);
        player.RotateHandle(moveDir);

        //if(player.GetClosestEnemyInRange()&& player.CheckAttackCooldown())
        //{
        //    player.GetClosestEnemyInRange().GetComponent<Enemy>().BeTargetted(true);
        //}

    }
}
