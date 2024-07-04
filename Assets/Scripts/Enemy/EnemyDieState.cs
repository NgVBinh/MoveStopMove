using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : EnemyState
{
    public EnemyDieState(Enemy enemy, EnemyStateMachine stateMachine, string animationName) : base(enemy, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.OnDeath?.Invoke();
        enemy.enemyDead = true;
        enemy.tag = "Untagged";
        enemy.col.enabled = false;
        enemy.EnemyIdle();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
