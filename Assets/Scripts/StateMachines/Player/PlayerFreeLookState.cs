using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int _freeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private const float AnimatorDampTime = 0.1f;
    private readonly bool _shouldFade;

    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        _shouldFade = shouldFade;
    }

    public override void Enter()
    {
        StateMachine.PlayerThirdPersonCameraController.canAim = true;
        StateMachine.SetCurrentRangedWeaponActive(false);
        // TODO rename (event naming convention)
        StateMachine.InputReader.TargetEvent += OnTarget;
        StateMachine.InputReader.JumpEvent += OnJump;
        StateMachine.InputReader.OnReloadWeapon += HandleReload;
        StartAnimation();

        if (StateMachine.ShouldHideSwordInFreeLookState) StateMachine.SetMeleeGameObjectsActive(false);
    }


    public override void Tick(float deltaTime)
    {
        if (!StateMachine.IsControllable) return;

        var hasEnoughStaminaToAttack = StateMachine.PlayerStamina.CurrentStamina > StateMachine.Attacks[0].StaminaCost;
        if (StateMachine.InputReader.IsAttacking && hasEnoughStaminaToAttack)
        {
            StateMachine.SwitchState(new PlayerAttackingState(StateMachine, 0));
            return;
        }

        if (StateMachine.InputReader.IsAiming)
        {
            StateMachine.SwitchState(new PlayerShootingState(StateMachine));
        }


        HandleMoveInput(deltaTime);
    }

    private void HandleMoveInput(float deltaTime)
    {
        var movement = CalculateMovement();
        Move(movement * StateMachine.FreeLookMovementSpeed, deltaTime);

        if (StateMachine.InputReader.MovementValue == Vector2.zero)
        {
            StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        StateMachine.Animator.SetFloat(_freeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        if (!StateMachine.InputReader.IsAiming)
            FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        StateMachine.InputReader.TargetEvent -= OnTarget;
        StateMachine.InputReader.JumpEvent -= OnJump;
        StateMachine.InputReader.OnReloadWeapon -= HandleReload;
    }

    private void OnJump()
    {
        if (StateMachine.PlayerStamina.CurrentStamina > StateMachine.JumpStaminaCost)
        {
            StateMachine.SwitchState(new PlayerJumpState(StateMachine));
        }
    }


    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        StateMachine.transform.rotation = Quaternion.Lerp(
            StateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * StateMachine.RotationDamping);
    }

    private void OnTarget()
    {
        if (!StateMachine.Targeter.SelectTarget()) return;
        StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
    }

    private void HandleReload()
    {
        if (StateMachine.CurrentRangedWeapon.TryReload())
        {
            StateMachine.SwitchState(new PlayerReloadingState(StateMachine));
        }
    }

    private void StartAnimation()
    {
        StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0f);

        if (_shouldFade)
            StateMachine.Animator.CrossFadeInFixedTime(_freeLookBlendTreeHash, DEFAULT_CROSS_FADE_DURATION);

        else
            StateMachine.Animator.Play(_freeLookBlendTreeHash);
    }
}