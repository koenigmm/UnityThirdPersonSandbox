using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    private readonly int _locomotionHash = Animator.StringToHash("WalkingBlendTree");
    private readonly int _speedHash = Animator.StringToHash("Speed");
    private const float WaypointDetectionTolerance = 0.25f;
    private const float DampTime = 0.4f;
    private const int StartIndex = 0;
    private float _timer;
    private int _currentWaypointIndex;
    private readonly Waypoint _waypoints;
    private bool _isDwelling;

    public EnemyPatrollingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _waypoints = stateMachine.Waypoints;
    }

    public override void Enter()
    {
        StateMachine.Agent.isStopped = false;
        _currentWaypointIndex = StartIndex;
        StateMachine.Agent.speed = StateMachine.WalkingSpeed;
        StateMachine.Agent.SetDestination(_waypoints.GetWaypoint(_currentWaypointIndex));
        StateMachine.Animator.Play(_locomotionHash);
    }

    public override void Tick(float deltaTime)
    {
        if (StateMachine.isAlarmed) StateMachine.SwitchState(new EnemyChasingState(StateMachine, true));
        
        if (HasReachedTargetPosition())
        {
            _isDwelling = true;
            _currentWaypointIndex = _waypoints.GetNextIndex(_currentWaypointIndex);
            _timer = 0f;
        }

        if (_timer >= StateMachine.WaypointDwellingTime)
        {
            StateMachine.Agent.SetDestination(_waypoints.GetWaypoint(_currentWaypointIndex));
            _isDwelling = false;
        }

        var animatorFloatValue = _isDwelling ? 0f : 1f;
        StateMachine.Animator.SetFloat(_speedHash, animatorFloatValue, DampTime, deltaTime);
        
        if (IsInChaseRange())
        {
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
            return;
        }

        _timer += deltaTime;
    }


    public override void Exit()
    {
        StateMachine.isAlarmed = false;
    }

    private bool HasReachedTargetPosition() =>
        Vector3.Distance(StateMachine.transform.position, _waypoints.GetWaypoint(_currentWaypointIndex)) <
        WaypointDetectionTolerance;
}