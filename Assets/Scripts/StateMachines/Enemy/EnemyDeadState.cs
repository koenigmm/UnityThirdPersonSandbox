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
        StateMachine.Animator.CrossFadeInFixedTime(_animationHash, BlendTime);
        DeactivateAI();
        DeactivateCollider();
    }

    private void DeactivateAI()
    {
        StateMachine.GetComponent<NavMeshAgent>().enabled = false;
        StateMachine.EnemyAI.enabled = false;
        StateMachine.Target.DestroyTarget();
    }

    private void DeactivateCollider()
    {
        StateMachine.GetComponent<Collider>().enabled = false;
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