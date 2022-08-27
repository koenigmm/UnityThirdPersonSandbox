using UnityEngine;

public class PlayerReloadingState : PlayerBaseState
{
    private readonly int _animationHash;
    private readonly bool _isAiming;
    private float _timer;
    private readonly float _reloadingTimeFromCurrentWeapon;
    
    public PlayerReloadingState(PlayerStateMachine stateMachine, bool isAiming = false) : base(stateMachine)
    {
        _animationHash = Animator.StringToHash("Reloading");
        _isAiming = isAiming;
        _reloadingTimeFromCurrentWeapon = StateMachine.CurrentRangedWeapon.ReloadingTime;
    }

    public override void Enter()
    {
        StateMachine.SetCurrentRangedWeaponActive(true);
        StateMachine.PlayerThirdPersonCameraController.canAim = true;
        StateMachine.Animator.CrossFadeInFixedTime(_animationHash, DEFAULT_CROSS_FADE_DURATION);
        StateMachine.SetMeleeGameObjectsActive(false);
    }

    public override void Tick(float deltaTime)
    {
        StateMachine.PlayerThirdPersonCameraController.LookAtAimTarget();
        var movement = CalculateMovement();
        Move(movement * StateMachine.FreeLookMovementSpeed, deltaTime);

        if (_timer >= _reloadingTimeFromCurrentWeapon && _isAiming)
            StateMachine.SwitchState(new PlayerShootingState(StateMachine));
        else if (_timer >= _reloadingTimeFromCurrentWeapon) StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));

        _timer += deltaTime;
    }

    public override void Exit()
    {
        StateMachine.SetCurrentRangedWeaponActive(false);
        StateMachine.PlayerThirdPersonCameraController.canAim = true;
        StateMachine.SetMeleeGameObjectsActive(true);
    }
}