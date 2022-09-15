using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
    private readonly int _speedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;
    private readonly bool _isProvoked;

    public EnemyChasingState(EnemyStateMachine stateMachine, bool isProvoked = false) : base(stateMachine)
    {
        _isProvoked = isProvoked;
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, DEFAULT_BLEND_TIME);
        StateMachine.Agent.isStopped = false;
        StateMachine.Agent.speed = StateMachine.ChasingSpeed;
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange() && !_isProvoked)
        {
            StateMachine.SwitchState(new EnemyIdleState(StateMachine));
            return;
        }

        if (!StateMachine.Player.GetComponent<Health>().IsAlive())
        {
            HandlePlayerDeath();
            return;
        }

        if (IsInAttackRange() && StateMachine.IsMelee)
        {
            StateMachine.SwitchState(new EnemyAttackingState(StateMachine));
            return;
        }
        
        if (IsInAttackRange() && !StateMachine.IsMelee)
        {
            StateMachine.transform.LookAt(StateMachine.Player.transform);
            StateMachine.SwitchState(new EnemyRangedAttack(StateMachine));
            return;
        }

        StateMachine.Animator.SetFloat(_speedHash, 1f, AnimatorDampTime, deltaTime);
        StateMachine.Agent.SetDestination(StateMachine.Player.transform.position);
    }

    public override void Exit() => StateMachine.Agent.isStopped = true;

    private void HandlePlayerDeath()
    {
        StateMachine.Agent.enabled = false;
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, 0.2f);
        StateMachine.Animator.SetFloat(_speedHash, 0f);
        StateMachine.enabled = false;
    }

    private bool IsInAttackRange() =>
        Vector3.Distance(StateMachine.Player.transform.position, StateMachine.transform.position) <=
        StateMachine.AttackRange;
}