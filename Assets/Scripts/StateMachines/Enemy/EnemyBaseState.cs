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

    protected void FacePlayer()
    {
        if (StateMachine.Player == null) return;
        var LookPos = StateMachine.Player.transform.position -
                      StateMachine.CharacterController.transform.position;

        LookPos.y = 0f;
        
        StateMachine.transform.rotation = Quaternion.LookRotation(LookPos);
    }
    
    protected void Move(Vector3 motion, float deltaTime)
    {
        StateMachine.CharacterController.Move((motion + StateMachine.ForceReceiver.Movement) * deltaTime);
    }
    
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }


}
