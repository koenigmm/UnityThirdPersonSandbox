using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine StateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    protected bool IsInChaseRange()
    {
        var playerDistanceToPlayerSqr = (StateMachine.Player.transform.position - StateMachine.transform.position).sqrMagnitude;

        return playerDistanceToPlayerSqr <= StateMachine.PlayerChasingRange * StateMachine.PlayerChasingRange;
    }


}
