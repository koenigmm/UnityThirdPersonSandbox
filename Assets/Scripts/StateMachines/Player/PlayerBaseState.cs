using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine StateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        StateMachine.CharacterController.Move((motion + StateMachine.ForceReceiver.Movement) * deltaTime);
    }
}
