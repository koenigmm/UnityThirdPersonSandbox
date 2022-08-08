using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int _impactHash = Animator.StringToHash("EnemyImpact");
    private const float CrossFadeDuration = 0.1f;
    private float _duration = 1f;
    
    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_impactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        _duration -= Time.deltaTime;

        if (_duration <= 0f)
        {
            StateMachine.SwitchState(new EnemyIdleState(StateMachine));
        }
    }

    public override void Exit()
    {
    }
}
