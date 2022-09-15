using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    private const float WaypointDetectionTolerance = 2.5f;
    private const int StartIndex = 0;
    private float _timer;
    private int _currentWaypointIndex;
    private readonly Waypoint _waypoints;

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
    }

    public override void Tick(float deltaTime)
    {
        if (StateMachine.isAlarmed) StateMachine.SwitchState(new EnemyChasingState(StateMachine, true));
        
        if (HasReachedTargetPosition())
        {
            _currentWaypointIndex = _waypoints.GetNextIndex(_currentWaypointIndex);
            _timer = 0f;
        }

        if (_timer >= StateMachine.WaypointDwellingTime)
        {
            StateMachine.Agent.SetDestination(_waypoints.GetWaypoint(_currentWaypointIndex));
        }
        
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