using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState currentState;
    public PlayerStateMachine() { }

    public void Initialize(PlayerState playerState)
    {
        currentState = playerState;
        currentState.Enter();
    }
    public void ChangeState(PlayerState playerState)
    {
        currentState.Exit();
        currentState = playerState;
        currentState.Enter();
    }
}
