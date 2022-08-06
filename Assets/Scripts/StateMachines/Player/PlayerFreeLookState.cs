using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int _freeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private const float AnimatorDampTime = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.InputReader.TargetEvent += OnTarget;
        StateMachine.Animator.Play(_freeLookBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
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
    }

    private void OnJump()
    {
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