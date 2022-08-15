using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private readonly int _animationClipHash = Animator.StringToHash("Jump");
    private const float AnimationBlendTime = 0.2f;
    private Vector3 _momentum;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.ForceReceiver.Jump(StateMachine.JumpForce);
        _momentum = StateMachine.CharacterController.velocity;
        _momentum.y = 0f;
        StateMachine.Animator.CrossFadeInFixedTime(_animationClipHash, AnimationBlendTime);
        StateMachine.PlayerStamina.ReduceStamina(StateMachine.JumpStaminaCost);

        StateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        StateMachine.SwitchState(new PlayerHangingState(StateMachine, ledgeForward, closestPoint));
    }

    public override void Tick(float deltaTime)
    {
       Move(_momentum, deltaTime);

       if (StateMachine.CharacterController.velocity.y <= 0f)
       {
           StateMachine.SwitchState(new PlayerFallingState(StateMachine));
           return;
       }
       
       FaceTarget();
    }

    public override void Exit()
    {
        StateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }
}
