using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private float _timer;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Agent.isStopped = true;
        StateMachine.Weapon.SetAttack(StateMachine.AttackDamage);
        StateMachine.Animator.CrossFadeInFixedTime(_attackHash, DEFAULT_BLEND_TIME);
        SetHealthBarCanvasActive(true);
    }

    public override void Tick(float deltaTime)
    {
        if (_timer >= StateMachine.AttackDelay)
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
        }

        _timer += deltaTime;
    }

    public override void Exit() => StateMachine.Agent.isStopped = false;
}