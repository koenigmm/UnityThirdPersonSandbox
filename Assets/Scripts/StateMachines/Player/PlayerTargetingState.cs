using UnityEngine;

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
        Debug.Log(StateMachine.Targeter.CurrentTarget.name);
    }

    public override void Exit()
    {
        StateMachine.InputReader.CancelEvent -= OnCancel;
    }

    private void OnCancel()
    {
        StateMachine.Targeter.Cancel();
        StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
    }
}