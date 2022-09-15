using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
    private readonly int _speedHash = Animator.StringToHash("Speed");
    private const float DampTime = 0.1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (StateMachine.Waypoints == null)
        {
            StateMachine.Agent.destination = StateMachine.DefaultPosition;
            StateMachine.Agent.isStopped = false;
        }
        
        // TODO Blend tree with walking animation
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, DEFAULT_BLEND_TIME);
        SetHealthBarCanvasActive(false);

        if (StateMachine.Waypoints != null) StateMachine.SwitchState(new EnemyPatrollingState(StateMachine));
    }

    public override void Tick(float deltaTime)
    {
        if (IsInChaseRange())
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
            return;
        }

        StateMachine.Animator.SetFloat(_speedHash, 0f, DampTime, deltaTime);
    }

    public override void Exit()
    {
    }
}