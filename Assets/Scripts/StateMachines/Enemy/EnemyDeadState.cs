using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    private readonly int _animationHash = Animator.StringToHash("EnemyDeath");
    private const float DeathAnimationBlendTime = 0.2f;

    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        DeactivateAI();
        StateMachine.Animator.CrossFadeInFixedTime(_animationHash, DeathAnimationBlendTime);
        SetHealthBarCanvasActive(false);
        DeactivateCollider();
        if (StateMachine.Sword != null) DeactivateSword();
    }

    private void DeactivateSword()
    {
        StateMachine.Sword.GetComponent<Collider>().enabled = false;
        StateMachine.Sword.enabled = false;
    }

    private void DeactivateAI()
    {
        StateMachine.Agent.enabled = false;
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