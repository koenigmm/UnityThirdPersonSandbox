using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
       
    }

    public override void Tick(float deltaTime)
    {
        var movement = CalculateMovement();
        StateMachine.CharacterController.Move(movement * (deltaTime * StateMachine.FreeLookMovementSpeed));

        if (StateMachine.InputReader.MovementValue == Vector2.zero)
        {
            StateMachine.Animator.SetFloat("FreeLookSpeed", 0, 0.1f, deltaTime);
            return;
        }
        
        StateMachine.Animator.SetFloat("FreeLookSpeed", 1, 0.1f, deltaTime);
        StateMachine.transform.rotation = Quaternion.LookRotation(movement);
    }

    public override void Exit()
    {
        
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
}