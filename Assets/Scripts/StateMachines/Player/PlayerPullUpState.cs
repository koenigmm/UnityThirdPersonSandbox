using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int _pullUpAnimationHash = Animator.StringToHash("Pullup");
    private readonly Vector3 _offset = new Vector3(0f, 2.325f, 0.65f);
    private const float CrossFadeBlendTime = 0.2f;

    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_pullUpAnimationHash, CrossFadeBlendTime);
        StateMachine.ForceReceiver.enabled = false;
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(StateMachine.Animator, "Climbing") < 1f) return;
        // TODO use normalizedTime in all other states
        if (StateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) return;

        StateMachine.CharacterController.enabled = false;
        StateMachine.transform.Translate(_offset, Space.Self);
        StateMachine.CharacterController.enabled = true;

        StateMachine.SwitchState(new PlayerFreeLookState(StateMachine, false));
    }

    public override void Exit()
    {
        StateMachine.CharacterController.Move(Vector3.zero);
        StateMachine.ForceReceiver.Reset();
        StateMachine.ForceReceiver.enabled = true;
    }
}