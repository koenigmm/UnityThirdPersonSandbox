using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int _impactHash = Animator.StringToHash("PlayerImpact");
    private const float CrossFadeDuration = 0.1f;
    private float _duration = 1f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_impactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        _duration -= deltaTime;

        if (_duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
    }
}
