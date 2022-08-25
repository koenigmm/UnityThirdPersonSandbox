using UnityEngine;

public class PlayerReloadingState : PlayerBaseState
{
    private readonly int _animationHash;
    private readonly bool _isAiming;
    private const float ReloadingTime = 1.5f;
    private float _timer;
    
    public PlayerReloadingState(PlayerStateMachine stateMachine, bool isAiming = false) : base(stateMachine)
    {
        _animationHash = Animator.StringToHash("Reloading");
        _isAiming = isAiming;
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_animationHash, DEFAULT_CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime)
    {
        var movement = CalculateMovement();
        Move(movement * StateMachine.FreeLookMovementSpeed, deltaTime);
        
        switch (_timer)
        {
            case >= ReloadingTime when _isAiming:
                StateMachine.SwitchState(new PlayerShootingState(StateMachine));
                break;
            case >= ReloadingTime:
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
                break;
        }
        
        _timer += deltaTime;
    }

    public override void Exit()
    {
        
    }
}