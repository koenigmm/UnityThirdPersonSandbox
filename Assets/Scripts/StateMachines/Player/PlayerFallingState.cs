using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int _animationClipHash = Animator.StringToHash("Fall");
    private Vector3 momentum;
    private float _blendTime;

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.PlayerThirdPersonCameraController.canAim = false;
        momentum = StateMachine.CharacterController.velocity;
        momentum.y = 0f;
        StateMachine.Animator.CrossFadeInFixedTime(_animationClipHash, _blendTime);

        StateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        StateMachine.SwitchState(new PlayerHangingState(StateMachine, ledgeForward, closestPoint));
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);
        if (StateMachine.CharacterController.isGrounded)
        {
            ReturnToLocomotion();
        }
        FaceTarget();
    }

    public override void Exit()
    {
        StateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }
}
