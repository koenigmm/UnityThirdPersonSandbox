using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int _hangingAnimationHash = Animator.StringToHash("Hanging");
    private const float CrossFadeDuration = 0.2f;
    private Vector3 _ledgeForward, _closestPoint;
    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    {
        _ledgeForward = ledgeForward;
        _closestPoint = closestPoint;
    }

    public override void Enter()
    {
        StateMachine.transform.rotation = Quaternion.LookRotation(_ledgeForward, Vector3.up);
        StateMachine.Animator.CrossFadeInFixedTime(_hangingAnimationHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (StateMachine.InputReader.MovementValue.y > 0f)
        {
            StateMachine.SwitchState(new PlayerPullUpState(StateMachine));
        }
        
        if (StateMachine.InputReader.MovementValue.y < 0f)
        {
            StateMachine.CharacterController.Move(Vector3.zero);
            StateMachine.ForceReceiver.Reset();
            StateMachine.SwitchState(new PlayerFallingState(StateMachine));
        }

    }

    public override void Exit()
    {
        
    }
}
