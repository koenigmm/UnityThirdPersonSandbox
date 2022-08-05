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
        var movement = new Vector3
        {
            x = StateMachine.InputReader.MovementValue.x,
            y = 0f,
            z = StateMachine.InputReader.MovementValue.y
        };
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
}