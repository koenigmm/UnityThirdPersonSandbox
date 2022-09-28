using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int _locomotionHash = Animator.StringToHash("WalkingBlendTree");
    private readonly int _speedHash = Animator.StringToHash("Speed");
    private const float DampTime = 0.1f;
    private readonly bool _isSuspicious;
    private float _timer;

    public EnemyIdleState(EnemyStateMachine stateMachine, bool isSuspicious = false) : base(stateMachine)
    {
        _isSuspicious = isSuspicious;
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, DEFAULT_BLEND_TIME);
    }

    public override void Tick(float deltaTime)
    {
        if (StateMachine.isAlarmed) StateMachine.SwitchState(new EnemyChasingState(StateMachine, true));

        _timer += deltaTime;
        
        if (_isSuspicious && _timer <= StateMachine.SuspiciousTime)
        {
            StateMachine.Animator.SetFloat(_speedHash, 0f);
            return;
        }

        if (StateMachine.Waypoints == null)
        {
            StateMachine.Agent.destination = StateMachine.DefaultPosition;
            StateMachine.Agent.isStopped = false;
        }
        else
        {
            StateMachine.SwitchState(new EnemyPatrollingState(StateMachine));
        }

        SetHealthBarCanvasActive(false);

        if (IsInChaseRange())
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
            return;
        }

        const float defaultPositionTolerance = 0.3f;
        var isAtDefaultPosition = Vector3.Distance(StateMachine.transform.position, StateMachine.DefaultPosition);
        StateMachine.Animator.SetFloat(_speedHash, isAtDefaultPosition > defaultPositionTolerance ? 1f : 0f);
    }

    public override void Exit()
    {
        StateMachine.isAlarmed = false;
    }
}