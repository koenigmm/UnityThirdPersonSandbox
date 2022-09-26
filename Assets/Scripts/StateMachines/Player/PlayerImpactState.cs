using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private float _timer;
    private const string ImpactClipName = "PlayerImpact";
    private float _length = 0.3f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.PlayerThirdPersonCameraController.canAim = false;
        StateMachine.Sword.enabled = false;
        HandleImpact();
        StateMachine.Animator.CrossFadeInFixedTime(ImpactClipName, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        _timer += deltaTime;
        if (_timer < StateMachine.ImpactDuration) return;

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