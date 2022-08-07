using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private const float TransitionDuration = 0.1f;
    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Weapon.SetAttack(StateMachine.AttackDamage, StateMachine.AttackKnockback);
        StateMachine.Animator.CrossFadeInFixedTime(_attackHash, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(StateMachine.Animator) >= 1)
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
    }

    public override void Exit()
    {
    }
}
