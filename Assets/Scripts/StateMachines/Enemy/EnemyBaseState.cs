using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine StateMachine;
    protected const float DEFAULT_BLEND_TIME = 0.1f;

    protected EnemyBaseState(EnemyStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    protected bool IsInChaseRange()
    {
        var distance = Vector3.Distance(StateMachine.transform.position, StateMachine.Player.transform.position);
        return distance < StateMachine.ChaseRange;
    }

    protected bool SetHealthBarCanvasActive(bool isActive)
    {
        if (StateMachine.HealthBarCanvas == null) return false;
        StateMachine.HealthBarCanvas.enabled = isActive;
        return true;

    }
}