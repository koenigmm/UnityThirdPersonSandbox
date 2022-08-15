using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyImpactState : EnemyBaseState
{
    private float _timer;
    private float _animationClipLength;
    private const float TransitionTime = 0.1f;
    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Agent.isStopped = true;
        StateMachine.Animator.CrossFadeInFixedTime("EnemyImpact", TransitionTime);
        _animationClipLength = StateMachine.Animator.GetCurrentAnimatorStateInfo(0).length;
        HandleImpact();
    }

    public override void Tick(float deltaTime)
    {
        _timer += deltaTime;

        if (_timer >= StateMachine.StunningDelay)
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
        }
            
    }

    public override void Exit()
    {
        StateMachine.Agent.isStopped = false;
    }
    
    private void HandleImpact()
    {
        StateMachine.enabled = false;
        StateMachine.transform.Translate(0, 0, StateMachine.ImpactOffset, Space.Self);
        StateMachine.enabled = true;
    }

    
}