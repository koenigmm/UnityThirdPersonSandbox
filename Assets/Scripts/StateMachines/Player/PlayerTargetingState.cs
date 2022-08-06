using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int _targetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.InputReader.CancelEvent += OnCancel;
        StateMachine.Animator.Play(_targetingBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (StateMachine.Targeter.CurrentTarget == null)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            return;
        }
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