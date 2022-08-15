using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private const float TransitionDuration = 0.1f;
    private const float AttackDelay = 0.85f;
    private float _timer;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Agent.isStopped = true;
        StateMachine.Weapon.SetAttack(StateMachine.AttackDamage);
        StateMachine.Animator.CrossFadeInFixedTime(_attackHash, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(_timer >= AttackDelay)
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
        
        _timer += deltaTime;
    }

    public override void Exit()
    {
        StateMachine.Agent.isStopped = false;
    }
}
