public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Ragdoll.ToggleRagdoll(true);
        StateMachine.Weapon.gameObject.SetActive(false);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}