using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int _blockAnimationHash = Animator.StringToHash("Block");
    private const float AnimationBlendTime = 0.2f;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Health.isInvulnerable = true;
        StateMachine.Animator.CrossFadeInFixedTime(_blockAnimationHash, AnimationBlendTime);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        if (!StateMachine.InputReader.IsBlocking)
        {
            StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
            return;
        }

        if (StateMachine.Targeter.CurrentTarget == null)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            return;
        }
    }

    public override void Exit()
    {
        StateMachine.Health.isInvulnerable = false;
    }
}