using UnityEngine;

public class EnemyRangedAttack : EnemyBaseState
{
    private float _timer;

    public EnemyRangedAttack(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter() => GameObject.Instantiate(StateMachine.Projectile,
        StateMachine.ProjectileLaunchPoint.position, StateMachine.ProjectileLaunchPoint.rotation);

    public override void Tick(float deltaTime)
    {
        if (_timer >= StateMachine.TimeBetweenRangedAttacks)
            StateMachine.SwitchState(new EnemyChasingState(StateMachine));
        _timer += deltaTime;
    }

    public override void Exit()
    {
        Debug.Log("Ranged Attack");
    }
}