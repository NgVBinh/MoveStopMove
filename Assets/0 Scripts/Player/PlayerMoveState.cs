using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
    }



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
        if (player.joystick.GetInputVector() != Vector2.zero) {
            Vector3 moveDir = new Vector3(player.joystick.GetInputVector().x, 0, player.joystick.GetInputVector().y);
            player.MoveHandle(moveDir * Time.deltaTime * player.moveSpeed);
            player.RotateHandle(moveDir);
        }
        else
            stateMachine.ChangeState(player.idleState);
        

        //if(player.GetClosestEnemyInRange()&& player.CheckAttackCooldown())
        //{
        //    player.GetClosestEnemyInRange().GetComponent<Enemy>().BeTargetted(true);
        //}

    }
}
