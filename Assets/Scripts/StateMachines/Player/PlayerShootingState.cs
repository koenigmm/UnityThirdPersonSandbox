using UnityEngine;

public class PlayerShootingState : PlayerBaseState
{
    private readonly int _shootingBlendTreeHash;
    private readonly int _shootingRunForwardHash;
    private readonly int _shootingRunRightHash;

    public PlayerShootingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        _shootingBlendTreeHash = Animator.StringToHash("ShootingBlendTree");
        _shootingRunForwardHash = Animator.StringToHash("ShootingRunForward");
        _shootingRunRightHash = Animator.StringToHash("ShootingRunRight");
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_shootingBlendTreeHash, DEFAULT_CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime)
    {
        if (!StateMachine.InputReader.IsAiming)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
        }
        
        UpdateAnimator(deltaTime);
        var movement = CalculateMovement();
        Move(movement * StateMachine.TargetingMovementSpeed, deltaTime);
        
        
    }

    public override void Exit()
    {
       
    }
    
    private void UpdateAnimator(float deltaTime)
    {
        const float dampTime = 0.1f;
        if (StateMachine.InputReader.MovementValue.y == 0)
        {
            StateMachine.Animator.SetFloat(_shootingRunForwardHash, 0, dampTime, deltaTime);
        }
        else
        {
            var value = StateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_shootingRunForwardHash, value, dampTime, deltaTime);
        }

        if (StateMachine.InputReader.MovementValue.x == 0)
        {
            StateMachine.Animator.SetFloat(_shootingRunRightHash, 0, dampTime, deltaTime);
        }
        else
        {
            var value = StateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            StateMachine.Animator.SetFloat(_shootingRunRightHash, value, dampTime, deltaTime);
        }
    }
}
