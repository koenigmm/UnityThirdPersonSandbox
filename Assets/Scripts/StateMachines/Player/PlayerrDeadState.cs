public class PlayerrDeadState : PlayerBaseState
{
    private const string DeadAnimationName = "PlayerDeathAnimation";
    private const float BlendTime = 0.1f;

    public PlayerrDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(DeadAnimationName, BlendTime);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}