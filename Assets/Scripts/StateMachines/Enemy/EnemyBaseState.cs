using UnityEngine;

public abstract class EnemyBaseState : State
{

    protected EnemyStateMachine StateMachine;
    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    
    protected bool IsInChaseRange()
    {
        var distance = Vector3.Distance(StateMachine.transform.position, StateMachine.Player.transform.position);
        return distance < StateMachine.ChaseRange;
    }
}
