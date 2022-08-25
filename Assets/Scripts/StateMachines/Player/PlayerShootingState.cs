using Unity.Mathematics;
using UnityEngine;

public class PlayerShootingState : PlayerBaseState
{
    private readonly int _shootingBlendTreeHash;
    private readonly int _shootingRunForwardHash;
    private readonly int _shootingRunRightHash;
    private bool _isShooting;

    public PlayerShootingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        _shootingBlendTreeHash = Animator.StringToHash("ShootingBlendTree");
        _shootingRunForwardHash = Animator.StringToHash("ShootingRunForward");
        _shootingRunRightHash = Animator.StringToHash("ShootingRunRight");
    }

    public override void Enter()
    {
        StateMachine.PlayerThirdPersonCameraController.canAim = true;
        StateMachine.Animator.CrossFadeInFixedTime(_shootingBlendTreeHash, DEFAULT_CROSS_FADE_DURATION);
        StateMachine.SetMeleeGameObjectsActive(false);
    }

    public override void Tick(float deltaTime)
    {
        if (!StateMachine.InputReader.IsAiming)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
        }

        if (StateMachine.InputReader.IsAttacking == false)
            _isShooting = false;

        if (StateMachine.InputReader.IsAttacking && !_isShooting)
        {
            var screenCenter = new Vector2(Screen.width * 0.5f , Screen.height * 0.5f);
            var rayToCenter = Camera.main.ScreenPointToRay(screenCenter);


            var hasHit = Physics.Raycast(rayToCenter, out var raycastHit, 900f, StateMachine.DefaultLayerMask);

            if (hasHit)
            {
                var toHitDirection = (raycastHit.point - StateMachine.DebugProjectileLaunchPoint.position).normalized;
                GameObject.Instantiate(StateMachine.DebugProjectile, StateMachine.DebugProjectileLaunchPoint.position, Quaternion.LookRotation(toHitDirection, Vector3.up));
                _isShooting = true;
            }
        }

        UpdateAnimator(deltaTime);
        var movement = CalculateMovement();
        Move(movement * StateMachine.TargetingMovementSpeed, deltaTime);
    }

    public override void Exit()
    {
        StateMachine.SetMeleeGameObjectsActive(true);
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