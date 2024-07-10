using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    private string animationName;

    protected float stateTimer;

    public PlayerState(Player player,PlayerStateMachine stateMachine,string animationName) {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animationName = animationName;
    }
    public virtual void Enter()
    {
        player.animator.SetBool(animationName, true);
    }
    public virtual void Exit()
    {
        player.animator.SetBool(animationName, false);

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
}
