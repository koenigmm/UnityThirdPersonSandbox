using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int _targetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int _targetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int _targetingRightHash = Animator.StringToHash("TargetingRight");
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
        if (StateMachine.InputReader.IsAttacking)
        {
            StateMachine.SwitchState(new PlayerAttackingState(StateMachine, 0));
            return;
        }
        if (StateMachine.Targeter.CurrentTarget == null)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            return;
        }

        var movement = CalculateMovement();
        Move(movement * StateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);
        
        FaceTarget();
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (StateMachine.InputReader.MovementValue.y == 0)
        {
            StateMachine.Animator.SetFloat(_targetingForwardHash, 0 ,0.1f, deltaTime);
        }
        else
        {
            var value = StateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_targetingForwardHash, value ,0.1f, deltaTime);
        }
        
        if (StateMachine.InputReader.MovementValue.x == 0)
        {
            StateMachine.Animator.SetFloat(_targetingRightHash, 0 ,0.1f, deltaTime);
        }
        else
        {
            var value = StateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_targetingRightHash, value ,0.1f, deltaTime);
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

    private Vector3 CalculateMovement()
    {
        var movement = new Vector3();

        var transform = StateMachine.transform;
        movement += transform.right * StateMachine.InputReader.MovementValue.x;
        movement += transform.forward * StateMachine.InputReader.MovementValue.y;

        return movement;
    }
}