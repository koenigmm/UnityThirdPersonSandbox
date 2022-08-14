using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        StateMachine.EnemyAI.enabled = true;
        StateMachine.Agent.enabled = true;
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, CrossFadeDuration);
        StateMachine.Agent.isStopped = false;
    }

    public override void Tick(float deltaTime)
    {

        if (!StateMachine.EnemyAI.IsInChaseRange())
        {
            StateMachine.SwitchState(new EnemyIdleState(StateMachine));
            return;
        }
        
        if (!StateMachine.Player.GetComponent<Health>().IsAlive())
        {
            HandlePlayerDeath();
            return;
        }
        
        if (IsInAttackRange())
        {
            StateMachine.SwitchState(new EnemyAttackingState(StateMachine));
        }

        StateMachine.Animator.SetFloat(_speedHash, 1f, AnimatorDampTime, deltaTime);
    }

    private void HandlePlayerDeath()
    {
        StateMachine.Agent.enabled = false;
        StateMachine.EnemyAI.enabled = false;
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash,0.2f);
        StateMachine.Animator.SetFloat(_speedHash, 0f);
        StateMachine.enabled = false;
    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(StateMachine.Player.transform.position, StateMachine.transform.position) <=
               StateMachine.AttackRange;
    }

    public override void Exit()
    {
        StateMachine.EnemyAI.enabled = false;
        StateMachine.Agent.enabled = false;
    }
    
}