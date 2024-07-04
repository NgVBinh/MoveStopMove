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
        player.canMove = false;

        stateTimer = player.attackDelay;
        attackDir =player.GetClosestEnemyInRange().transform.position - player.transform.position;
        attackDir.y = 0;
        player.RotateHandle(attackDir);


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0 && player.CheckAttackCooldown())
        {
            player.StartCoroutine(player.HideWeaponOnAttack(1));
            player.ThrowWeapon(attackDir);
            player.canMove = true;
            player.SetCooldown();

        }

        if (player.joystick.Direction != Vector2.zero && player.canMove)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }


}
