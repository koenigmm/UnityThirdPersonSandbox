public class PlayerrDeadState : PlayerBaseState
{
    private const string DeadAnimationName = "PlayerDeathAnimation";

    public PlayerrDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter() =>
        StateMachine.Animator.CrossFadeInFixedTime(DeadAnimationName, DEFAULT_CROSS_FADE_DURATION);

    public override void Tick(float deltaTime) => Move(deltaTime);

    public override void Exit()
    {
    }
}