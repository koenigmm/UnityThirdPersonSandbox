using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected readonly PlayerStateMachine StateMachine;
    protected const float DEFAULT_CROSS_FADE_DURATION = 0.2f;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        StateMachine.CharacterController.Move((motion + StateMachine.ForceReceiver.Movement) * deltaTime);
    }
    
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void FaceTarget()
    {
        if (StateMachine.Targeter.CurrentTarget == null) return;
        var LookPos = StateMachine.Targeter.CurrentTarget.transform.position -
                         StateMachine.CharacterController.transform.position;

        LookPos.y = 0f;
        
        StateMachine.transform.rotation = Quaternion.LookRotation(LookPos);
    }

    protected void ReturnToLocomotion()
    {
        if (StateMachine.Targeter.CurrentTarget != null)
        {
            StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
        }
        else
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
        }
    }
    
    protected Vector3 CalculateMovement()
    {
        var forward = StateMachine.MainCameraTransform.forward;
        var right = StateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * StateMachine.InputReader.MovementValue.y + right * StateMachine.InputReader.MovementValue.x;
    }
    
}
