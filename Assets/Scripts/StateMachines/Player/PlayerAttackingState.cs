using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack _attack;
    private float _previousFrameTime;
    private bool alreadyAppliedForce;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        _attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration);
        StateMachine.Weapon.SetAttack(_attack.Damage);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        var normalizedTime = GetNormalizedTime();

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= _attack.ForceTime)
                TryApplyForce();

            if (StateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (StateMachine.Targeter.CurrentTarget != null)
            {
                StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
            }
            else
            {
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            }
        }

        _previousFrameTime = normalizedTime;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (_attack.ComboStateIndex == -1) return;
        if (normalizedTime < _attack.ComboAttackTime) return;

        StateMachine.SwitchState
        (
            new PlayerAttackingState
            (
                StateMachine,
                _attack.ComboStateIndex
            )
        );
    }

    public override void Exit()
    {
    }

    private float GetNormalizedTime()
    {
        var currentInfo = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        var nextInfo = StateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (StateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }

        else if (!StateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }

        else
        {
            return 0f;
        }
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;
        StateMachine.ForceReceiver.AddForce(StateMachine.transform.forward * _attack.Force);
        alreadyAppliedForce = true;
    }
}