using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private readonly int _animationClipHash = Animator.StringToHash("Jump");
    private const float AnimationBlendTime = 0.2f;
    private Vector3 momentum;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.ForceReceiver.Jump(StateMachine.JumpForce);
        momentum = StateMachine.CharacterController.velocity;
        momentum.y = 0f;
        StateMachine.Animator.CrossFadeInFixedTime(_animationClipHash, AnimationBlendTime);
    }

    public override void Tick(float deltaTime)
    {
       Move(momentum, deltaTime);

       if (StateMachine.CharacterController.velocity.y <= 0f)
       {
           StateMachine.SwitchState(new PlayerFallingState(StateMachine));
           return;
       }
       
       FaceTarget();
    }

    public override void Exit()
    {
       
    }
}
