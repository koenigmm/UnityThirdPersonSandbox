using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int _dodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int _dodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int _dodgeRightHash = Animator.StringToHash("DodgeRight");
    private Vector3 _dodgingDirectionInput;
    private float _remainingDodgeTime;
    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
    {
        _dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter()
    {
        StateMachine.PlayerThirdPersonCameraController.canAim = false;
        _remainingDodgeTime = StateMachine.DodgeDuration;

        StateMachine.Animator.SetFloat(_dodgeForwardHash, _dodgingDirectionInput.y);
        StateMachine.Animator.SetFloat(_dodgeRightHash, _dodgingDirectionInput.x);
        StateMachine.Animator.CrossFadeInFixedTime(_dodgeBlendTreeHash, DEFAULT_CROSS_FADE_DURATION);

        StateMachine.Health.isInvulnerable = true;
        StateMachine.PlayerStamina.ReduceStamina(StateMachine.DodgeStaminaCost);
    }

    public override void Tick(float deltaTime)
    {
        var movement = new Vector3();
        var transform = StateMachine.transform;
        movement += transform.right * (_dodgingDirectionInput.x * StateMachine.DodgeLength) / StateMachine.DodgeDuration;
        movement += transform.forward * (_dodgingDirectionInput.y * StateMachine.DodgeLength) / StateMachine.DodgeDuration;
        
        Move(movement, deltaTime);
        FaceTarget();
        _remainingDodgeTime -= deltaTime;
        
        if (_remainingDodgeTime <= 0f)
            StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
    }

    public override void Exit()
    {
        StateMachine.Health.isInvulnerable = false;
    }
}
