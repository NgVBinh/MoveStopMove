
using UnityEngine.AI;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    public EnemyMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animationName) : base(enemy, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.speed = enemy.moveSpeed;
        //ebug.Log(randomPoint);
        enemy.SetRandomDestination();

        //const var
        stateTimer =Random.Range(2,6);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetRandomDestination();
        //if (enemy.GetClosestTargetInRange() && enemy.CheckAttackCooldown())
        //{
        //    stateMachine.ChangeState(enemy.enemyAttackState);
        //}

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.enemyIdleState);

        }
    }

}
