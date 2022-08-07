public class PlayerAttackingState : PlayerBaseState
{
    private Attack _attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackID) : base(stateMachine)
    {
        _attack = stateMachine.Attacks[attackID];
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
