using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private float _animationClipLength;
    private float _timer;
    private const string ImpactClipName = "PlayerImpact";

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.PlayerThirdPersonCameraController.canAim = false;
        HandleImpact();
        StateMachine.Animator.CrossFadeInFixedTime(ImpactClipName, 0.1f);
        _animationClipLength = FindAnimationClipLength();
        _animationClipLength *= StateMachine.Animator.GetCurrentAnimatorStateInfo(0).speedMultiplier;
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        _timer += deltaTime;
        if (_timer < _animationClipLength) return;

        if (StateMachine.Targeter.CurrentTarget == null)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
        }
        else
        {
            StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
        }
    }

    public override void Exit()
    {
        
    }

    private void HandleImpact()
    {
        StateMachine.CharacterController.Move(StateMachine.transform.forward * StateMachine.ImpactDistance);
    }

    private float FindAnimationClipLength()
    {
        foreach (var animationClip in StateMachine.Animator.runtimeAnimatorController.animationClips)
        {
            if (animationClip.name == ImpactClipName)
            {
                return animationClip.length;
            }
        }

        return 0f;
    }
}