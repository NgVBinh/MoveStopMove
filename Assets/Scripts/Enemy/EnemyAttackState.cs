using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animationName) : base(enemy, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Attack player");
        stateTimer = enemy.attackCooldown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.agent.SetDestination(enemy.transform.position);// enemy dung im

        if (stateTimer > 0) return;
        if (!enemy.CheckPlayerInRange())
        {
            stateMachine.ChangeState(enemy.enemyMoveState);
        }
        else
        {
            stateMachine.ChangeState(enemy.enemyAttackState);
        }
    }
}
