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

    protected void AlarmNearbyEnemies()
    {
        var rayCastHits =
            Physics.SphereCastAll(StateMachine.transform.position, StateMachine.AlarmRange, Vector3.up, 0f);

        foreach (var hit in rayCastHits)
        {
            if (!hit.collider.TryGetComponent(out EnemyStateMachine enemyStateMachine)) continue;
            enemyStateMachine.isAlarmed = true;
        }
    }
}