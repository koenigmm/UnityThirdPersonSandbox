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

        var movement = CalculateMovement();
        Move(movement * StateMachine.TargetingMovementSpeed, deltaTime);
        
        FaceTarget();
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

    private Vector3 CalculateMovement()
    {
        var movement = new Vector3();

        var transform = StateMachine.transform;
        movement += transform.right * StateMachine.InputReader.MovementValue.x;
        movement += transform.forward * StateMachine.InputReader.MovementValue.y;

        return movement;
    }
}