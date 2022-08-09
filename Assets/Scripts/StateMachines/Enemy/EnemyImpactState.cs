using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyImpactState : EnemyBaseState
{
    private float _timer;
    private float _animationClipLength;
    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.Play("EnemyImpact");
        _animationClipLength = StateMachine.Animator.GetCurrentAnimatorStateInfo(0).length;
        StateMachine.EnemyAI.HandleImpact();
    }

    public override void Tick(float deltaTime)
    {
        _timer += deltaTime;

        if (_timer >= _animationClipLength)
        {
            StateMachine.EnemyAI.ActivateAI();
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
        }
            
    }

    public override void Exit()
    {
    }

    
}