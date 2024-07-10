using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private Vector3 attackDir;
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.rb.velocity = Vector3.zero;

        attackDir = player.enemyTargetted.position - player.transform.position;
        attackDir.y = 0;

        stateTimer = player.attackDelay;
        player.RotateHandle(attackDir);


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if (player.joystick.Direction != Vector2.zero && player.canMove)
        if (player.joystick.GetInputVector() != Vector2.zero && player.canMove)
        {
            stateMachine.ChangeState(player.moveState);
        }

        if (stateTimer < 0 && player.CheckAttackCooldown())
        {
            player.StartCoroutine(player.HideWeaponOnAttack(1));
            player.Attack(attackDir);
            player.SetCooldown();

        }

    }

}
