using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int _hangingAnimationHash = Animator.StringToHash("Hanging");
    private Vector3 _ledgeForward;
    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    {
        _ledgeForward = ledgeForward;
    }

    public override void Enter()
    {
        StateMachine.PlayerThirdPersonCameraController.canAim = false;
        StateMachine.transform.rotation = Quaternion.LookRotation(_ledgeForward, Vector3.up);
        StateMachine.Animator.CrossFadeInFixedTime(_hangingAnimationHash, DEFAULT_CROSS_FADE_DURATION);
        StateMachine.ForceReceiver.enabled = false;
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
        StateMachine.ForceReceiver.enabled = true;
    }
}
