using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
    private readonly int _speedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f;
    private const float DampTime = 0.1f;
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        
    }
    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (IsInChaseRange())
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
            return;
        }
        
        StateMachine.Animator.SetFloat(_speedHash, 0f, DampTime, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        StateMachine.CharacterController.Move((motion + StateMachine.ForceReceiver.Movement) * deltaTime);
    }
    
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    public override void Exit()
    {
    }
}
