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
        _reloadingTimeFromCurrentWeapon = StateMachine.CurrentWeapon.ReloadingTime;
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_animationHash, DEFAULT_CROSS_FADE_DURATION);
        StateMachine.SetMeleeGameObjectsActive(false);
    }

    public override void Tick(float deltaTime)
    {
        var movement = CalculateMovement();
        Move(movement * StateMachine.FreeLookMovementSpeed, deltaTime);

        if (_timer >= _reloadingTimeFromCurrentWeapon && _isAiming)
            StateMachine.SwitchState(new PlayerShootingState(StateMachine));
        else if (_timer >= _reloadingTimeFromCurrentWeapon) StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));

        _timer += deltaTime;
    }

    public override void Exit()
    {
        StateMachine.SetMeleeGameObjectsActive(true);
    }
}