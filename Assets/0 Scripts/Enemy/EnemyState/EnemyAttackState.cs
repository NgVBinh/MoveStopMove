using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private Vector3 attackDir;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animationName) : base(enemy, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //enemy.canMove = false;
        enemy.EnemyIdle();
        attackDir = enemy.GetClosestTargetInRange().position - enemy.transform.position;
        attackDir.y = 0;

        stateTimer = enemy.attackDelay;
        enemy.RotateHandle(attackDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0) return;

        if (stateTimer < 0 && enemy.CheckAttackCooldown())
        {
            enemy.StartCoroutine(enemy.HideWeaponOnAttack(1));
            enemy.Attack(attackDir);
            enemy.SetCooldown();

        }

        if (Random.Range(0, 2) == 0) { stateMachine.ChangeState(enemy.enemyIdleState); }
        else { stateMachine.ChangeState(enemy.enemyMoveState); }

        //if (!enemy.GetClosestTargetInRange())
        //{
        //    stateMachine.ChangeState(enemy.enemyMoveState);
        //}
        //else
        //{
        //    stateMachine.ChangeState(enemy.enemyAttackState);
        //}
    }
}
