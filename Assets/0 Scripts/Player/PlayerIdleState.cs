using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        player.rb.velocity = Vector3.zero;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(player.joystick.Direction!=Vector2.zero && player.canMove)
        {
            stateMachine.ChangeState(player.moveState);
        }

        // attack enemy
        if (player.enemyTargetted!=null && player.CheckAttackCooldown())
        {
            //player.GetClosestEnemyInRange().GetComponent<Enemy>().BeTargetted(true);

            stateMachine.ChangeState(player.attackState);
        }
    }
}
