using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int _freeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    private bool _shouldFade;

    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        _shouldFade = shouldFade;
    }

    public override void Enter()
    {
        StateMachine.InputReader.TargetEvent += OnTarget;
        StateMachine.InputReader.JumpEvent += OnJump;
        StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0f);
        
        if (_shouldFade)
            StateMachine.Animator.CrossFadeInFixedTime(_freeLookBlendTreeHash, CrossFadeDuration);
        
        else 
            StateMachine.Animator.Play(_freeLookBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (StateMachine.InputReader.IsAttacking && StateMachine.PlayerStamina.CurrentStamina > 0)
        {
            StateMachine.SwitchState(new PlayerAttackingState(StateMachine, 0));
            return;
        }
        
        var movement = CalculateMovement();
        Move(movement * StateMachine.FreeLookMovementSpeed, deltaTime);

        if (StateMachine.InputReader.MovementValue == Vector2.zero)
        {
            StateMachine.Animator.SetFloat(_freeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        StateMachine.Animator.SetFloat(_freeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        StateMachine.InputReader.TargetEvent -= OnTarget;
        StateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump()
    {
        if (StateMachine.PlayerStamina.CurrentStamina > StateMachine.JumpStaminaCost)
        {
            StateMachine.SwitchState(new PlayerJumpState(StateMachine));
        }
    }

    private Vector3 CalculateMovement()
    {
        var forward = StateMachine.MainCameraTransform.forward;
        var right = StateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * StateMachine.InputReader.MovementValue.y + right * StateMachine.InputReader.MovementValue.x;
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
}