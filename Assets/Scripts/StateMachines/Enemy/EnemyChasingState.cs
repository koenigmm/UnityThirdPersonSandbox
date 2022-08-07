using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
    private readonly int _speedHash = Animator.StringToHash("Speed");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            StateMachine.SwitchState(new EnemyIdleState(StateMachine));
            return;
        }
        MoveToPlayer(deltaTime);
        StateMachine.Animator.SetFloat(_speedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        StateMachine.Agent.ResetPath();
    }

    private void MoveToPlayer(float deltaTime)
    {
        StateMachine.Agent.destination = StateMachine.Player.transform.position;

        var targetVector = StateMachine.Agent.desiredVelocity.normalized * StateMachine.MovementSpeed;
        Move(targetVector, deltaTime);

        StateMachine.Agent.velocity = StateMachine.CharacterController.velocity;
    }
    
    // TODO move in parent class
    protected void Move(Vector3 motion, float deltaTime)
    {
        StateMachine.CharacterController.Move((motion + StateMachine.ForceReceiver.Movement) * deltaTime);
    }
    
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }
}
