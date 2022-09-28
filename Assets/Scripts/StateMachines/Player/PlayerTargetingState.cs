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
        StateMachine.Animator.CrossFadeInFixedTime(_targetingBlendTreeHash, DEFAULT_CROSS_FADE_DURATION);
        StateMachine.InputReader.DodgeEvent += OnDodge;
        StateMachine.InputReader.TargetEvent += OnTarget;
        StateMachine.InputReader.JumpEvent += OnJump;
        StateMachine.PlayerThirdPersonCameraController.canAim = false;
        
        StateMachine.SetMeleeGameObjectsActive(true);
    }

    public override void Tick(float deltaTime)
    {
        if (!StateMachine.IsControllable) return;
        
        if (StateMachine.InputReader.IsAiming)
        {
            StateMachine.SwitchState(new PlayerShootingState(StateMachine));
            return;
        }
        
        var hasEnoughStaminaToAttack = StateMachine.PlayerStamina.CurrentStamina >= StateMachine.Attacks[0].StaminaCost;
        
        if (StateMachine.InputReader.IsAttacking && hasEnoughStaminaToAttack)
        {
            StateMachine.SwitchState(new PlayerAttackingState(StateMachine, 0));
            return;
        }

        if (StateMachine.Targeter.CurrentTarget == null)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            return;
        }

        var hasEnoughStaminaToBlock = StateMachine.PlayerStamina.CurrentStamina >= StateMachine.BlockingStaminaCost;
        if (StateMachine.InputReader.IsBlocking && hasEnoughStaminaToBlock)
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
        const float dampTime = 0.1f;
        if (StateMachine.InputReader.MovementValue.y == 0)
        {
            StateMachine.Animator.SetFloat(_targetingForwardHash, 0, dampTime, deltaTime);
        }
        else
        {
            var value = StateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_targetingForwardHash, value, dampTime, deltaTime);
        }

        if (StateMachine.InputReader.MovementValue.x == 0)
        {
            StateMachine.Animator.SetFloat(_targetingRightHash, 0, dampTime, deltaTime);
        }
        else
        {
            var value = StateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_targetingRightHash, value, dampTime, deltaTime);
        }
    }

    public override void Exit()
    {
        StateMachine.InputReader.TargetEvent -= OnTarget;
        StateMachine.InputReader.DodgeEvent -= OnDodge;
        StateMachine.InputReader.JumpEvent -= OnJump;
        
        if (StateMachine.ShouldHideSwordInFreeLookState) StateMachine.SetMeleeGameObjectsActive(false);
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