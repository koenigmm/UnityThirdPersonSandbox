using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private const float TransitionDuration = 0.1f;
    private const float WaitTime = 0.85f;
    private float _timer;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Weapon.SetAttack(StateMachine.AttackDamage);
        StateMachine.Animator.CrossFadeInFixedTime(_attackHash, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        StateMachine.EnemyAI.enabled = false;
        if(_timer >= WaitTime)
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
        
        _timer += deltaTime;
    }

    public override void Exit()
    {
    }
}
