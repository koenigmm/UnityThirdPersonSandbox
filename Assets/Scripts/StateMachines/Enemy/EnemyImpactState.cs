using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private float _timer;
    
    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Agent.isStopped = true;
        StateMachine.Animator.CrossFadeInFixedTime("EnemyImpact", DEFAULT_BLEND_TIME);
        HandleImpact();
    }

    public override void Tick(float deltaTime)
    {
        if (_timer >= StateMachine.StunningDelay) StateMachine.SwitchState(new EnemyChasingState(StateMachine, true));

        _timer += deltaTime;
    }

    public override void Exit() => StateMachine.Agent.isStopped = false;

    private void HandleImpact()
    {
        StateMachine.enabled = false;
        StateMachine.transform.Translate(0, 0, StateMachine.ImpactOffset, Space.Self);
        StateMachine.enabled = true;
    }

    
}