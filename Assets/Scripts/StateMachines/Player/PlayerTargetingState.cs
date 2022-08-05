public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.InputReader.CancelEvent += OnCancel;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        StateMachine.InputReader.CancelEvent -= OnCancel;
    }

    private void OnCancel()
    {
        StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
    }
}