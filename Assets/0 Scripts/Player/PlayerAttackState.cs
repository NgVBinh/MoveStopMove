using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private Vector3 attackDir;
    private bool attacked;
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        attacked = false;
        base.Enter();
        player.canMove = false;
        attackDir = player.enemyTargetted.position - player.transform.position;
        stateTimer = player.attackDelay;
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

        if (player.joystick.Direction != Vector2.zero && player.canMove)
        {
            stateMachine.ChangeState(player.moveState);
        }
        if (attacked) return;

        if (stateTimer < 0 && player.CheckAttackCooldown())
        {
            player.StartCoroutine(player.HideWeaponOnAttack(1));
            if (player.enemyTargetted != null)
            {
                attackDir = player.enemyTargetted.position - player.transform.position;
            }
            player.ThrowMultiWeapon(attackDir, 5, 15);
            player.canMove = true;
            player.SetCooldown();
            attacked = true;

        }



        
    }


}
