using UnityEngine;
using UnityEngine.AI;

public class EnemyDeadState : EnemyBaseState
{
    private readonly int _animationHash = Animator.StringToHash("EnemyDeath");
    private const float BlendTime = 0.2f;

    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // TODO merge with EnemyAI Script
        DeactivateAI();
        StateMachine.Animator.CrossFadeInFixedTime(_animationHash, BlendTime);
        DeactivateCollider();
    }

    private void DeactivateAI()
    {
        StateMachine.Agent.enabled = false;
        StateMachine.EnemyAI.enabled = false;
        StateMachine.Target.DestroyTarget();
    }

    private void DeactivateCollider()
    {
        StateMachine.Collider.enabled = false;
        foreach (var collider in StateMachine.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
}