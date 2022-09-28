using UnityEngine;

public class EnemyRangedAttack : EnemyBaseState
{
    private float _timer;
    private readonly int _animationNameHash = Animator.StringToHash("Shooting");

    public EnemyRangedAttack(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_animationNameHash, DEFAULT_BLEND_TIME);
        GameObject.Instantiate(StateMachine.Projectile,
            StateMachine.ProjectileLaunchPoint.position, StateMachine.ProjectileLaunchPoint.rotation);

        StateMachine.MuzzleFlashVFX.enabled = true;
        StateMachine.MuzzleFlashVFX.Play();
    }

    public override void Tick(float deltaTime)
    {
        if (_timer >= StateMachine.TimeBetweenRangedAttacks)
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
        _timer += deltaTime;
    }

    public override void Exit()
    {
        Debug.Log("Ranged Attack");
        StateMachine.MuzzleFlashVFX.enabled = false;
    }
}