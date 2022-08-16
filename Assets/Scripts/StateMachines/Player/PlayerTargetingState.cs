using System;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int _targetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int _targetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int _targetingRightHash = Animator.StringToHash("TargetingRight");
    private const float CrossFadeDuration = 0.1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_targetingBlendTreeHash, CrossFadeDuration);
        StateMachine.InputReader.DodgeEvent += OnDodge;
        StateMachine.InputReader.TargetEvent += OnTarget;
        StateMachine.InputReader.JumpEvent += OnJump;
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

        var hasEnoughStamina = StateMachine.PlayerStamina.CurrentStamina >= StateMachine.BlockingStaminaCost;
        if (StateMachine.InputReader.IsBlocking && hasEnoughStamina)
        {
            StateMachine.SwitchState(new PlayerBlockingState(StateMachine));
        }

        var movement = CalculateMovement(deltaTime);
        Move(movement * StateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (StateMachine.InputReader.MovementValue.y == 0)
        {
            StateMachine.Animator.SetFloat(_targetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            var value = StateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_targetingForwardHash, value, 0.1f, deltaTime);
        }

        if (StateMachine.InputReader.MovementValue.x == 0)
        {
            StateMachine.Animator.SetFloat(_targetingRightHash, 0, 0.1f, deltaTime);
        }
        else
        {
            var value = StateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_targetingRightHash, value, 0.1f, deltaTime);
        }
    }

    public override void Exit()
    {
        StateMachine.InputReader.TargetEvent -= OnTarget;
        StateMachine.InputReader.DodgeEvent -= OnDodge;
        StateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump()
    {
        if (StateMachine.PlayerStamina.CurrentStamina > StateMachine.JumpStaminaCost)
        {
            StateMachine.SwitchState(new PlayerJumpState(StateMachine));
        }
    }

    private void OnTarget()
    {
        StateMachine.Targeter.Cancel();
        StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
    }

    private void OnDodge()
    {
        if (StateMachine.InputReader.MovementValue == Vector2.zero) return;
        if (StateMachine.PlayerStamina.CurrentStamina < StateMachine.DodgeStaminaCost) return;
        StateMachine.SwitchState(new PlayerDodgingState(StateMachine, StateMachine.InputReader.MovementValue));
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        var movement = new Vector3();
        var transform = StateMachine.transform;

        movement += transform.right * StateMachine.InputReader.MovementValue.x;
        movement += transform.forward * StateMachine.InputReader.MovementValue.y;

        return movement;
    }
}