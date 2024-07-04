using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animationName) : base(enemy, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = stateTimer = Random.Range(1,3);

        enemy.EnemyIdle();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer < 0) {
            enemy.stateMachine.ChangeState(enemy.enemyMoveState);
        }

        if (enemy.GetClosestTargetInRange() && enemy.CheckAttackCooldown())
        {
            enemy.stateMachine.ChangeState(enemy.enemyAttackState);
        }
    }
}
