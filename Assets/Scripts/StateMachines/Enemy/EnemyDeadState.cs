using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Ragdoll.ToggleRagdoll(true);
        StateMachine.Weapon.gameObject.SetActive(false);
        GameObject.Destroy(StateMachine.Target);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}