using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
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
    }

    public override void Tick(float deltaTime)
    {
        if (StateMachine.isAlarmed) StateMachine.SwitchState(new EnemyChasingState(StateMachine, true));
        
        _timer += deltaTime;
        if (_isSuspicious && _timer <= StateMachine.SuspiciousTime) return;

        if (StateMachine.Waypoints == null)
        {
            StateMachine.Agent.destination = StateMachine.DefaultPosition;
            StateMachine.Agent.isStopped = false;
        }
        else StateMachine.SwitchState(new EnemyPatrollingState(StateMachine));
        

        // TODO Blend tree with walking animation
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, DEFAULT_BLEND_TIME);
        SetHealthBarCanvasActive(false);


        if (IsInChaseRange())
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
            return;
        }

        StateMachine.Animator.SetFloat(_speedHash, 0f, DampTime, deltaTime);
    }

    public override void Exit()
    {
        StateMachine.isAlarmed = false;
    }
}