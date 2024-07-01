
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
        Vector3 randomPoint = GetRandomPointOnNavMesh();
        //ebug.Log(randomPoint);
        enemy.agent.SetDestination(randomPoint);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        SetRandomDestination();
        if (enemy.CheckPlayerInRange())
        {
            stateMachine.ChangeState(enemy.enemyAttackState);
        }
    }

    private void SetRandomDestination()
    {
        // Debug.Log(enemy.agent.pathPending+"   "+ enemy.agent.remainingDistance);

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.5f)
        {
            enemy.agent.SetDestination(GetRandomPointOnNavMesh());
        }
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
        NavMeshHit hit;
        Vector3 randomPoint = enemy.transform.position + Random.insideUnitSphere * 30f;

        if (NavMesh.SamplePosition(randomPoint, out hit, 30f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // If a random point is not on NavMesh, try again.
        return GetRandomPointOnNavMesh();
    }



}
