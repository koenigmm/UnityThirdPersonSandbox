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
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, CrossFadeDuration);
        StateMachine.EnemyAI.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public override void Tick(float deltaTime)
    {
        if (!StateMachine.EnemyAI.IsInChaseRange())
        {
            StateMachine.SwitchState(new EnemyIdleState(StateMachine));
            return;
        }
        
        Debug.Log(IsInAttackRange());

        if (IsInAttackRange())
        {
            StateMachine.SwitchState(new EnemyAttackingState(StateMachine));
            Debug.Log("Is in Attack Range");
        }
        else
        {
        }
        
        StateMachine.Animator.SetFloat(_speedHash, 1f, AnimatorDampTime, deltaTime);
    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(StateMachine.Player.transform.position, StateMachine.transform.position) <=
               StateMachine.AttackRange;
    }

    public override void Exit()
    {
        StateMachine.EnemyAI.GetComponent<NavMeshAgent>().isStopped = true;
    }
    
}