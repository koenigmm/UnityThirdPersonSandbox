using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private const float InterpolationRatio = 10f;
    private float _timer;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Agent.isStopped = true;
        StateMachine.Sword.SetAttack(StateMachine.AttackDamage);
        StateMachine.Animator.CrossFadeInFixedTime(_attackHash, DEFAULT_BLEND_TIME);
        SetHealthBarCanvasActive(true);
    }

    public override void Tick(float deltaTime)
    {
        // Face Player
        var lookRotation = Quaternion.LookRotation((StateMachine.Player.transform.position - StateMachine.transform.position).normalized);
        StateMachine.transform.rotation = Quaternion.Lerp(StateMachine.transform.rotation, lookRotation, deltaTime * InterpolationRatio );
        
        if (_timer >= StateMachine.AttackDelay)
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
        }

        _timer += deltaTime;
    }

    public override void Exit() => StateMachine.Agent.isStopped = false;
}