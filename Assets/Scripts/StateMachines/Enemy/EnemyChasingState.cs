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
        
        if (IsInAttackRange())
            StateMachine.SwitchState(new EnemyAttackingState(StateMachine));
        
        MoveToPlayer(deltaTime);
        FacePlayer();
        StateMachine.Animator.SetFloat(_speedHash, 1f, AnimatorDampTime, deltaTime);
    }

    private bool IsInAttackRange()
    {
        var playerDistanceSqr =
            (StateMachine.Player.transform.position - StateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= StateMachine.AttackRange * StateMachine.AttackRange;
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
}
