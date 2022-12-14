using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
    private readonly int _speedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;
    private bool _isProvoked;
    private const float ProvocationDuration = 10f;
    private float _timer;

    public EnemyChasingState(EnemyStateMachine stateMachine, bool isProvoked = false) : base(stateMachine)
    {
        _isProvoked = isProvoked;
    }

    public override void Enter()
    {
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, DEFAULT_BLEND_TIME);
        StateMachine.Agent.isStopped = false;
        StateMachine.Agent.speed = StateMachine.ChasingSpeed;
        AlarmNearbyEnemies();
    }

    public override void Tick(float deltaTime)
    {
        if (_isProvoked) _timer += deltaTime;
        
        if (!StateMachine.Player.GetComponent<Health>().IsAlive())
        {
            HandlePlayerDeath();
            return;
        }
        
        if (_timer > ProvocationDuration)
        {
            _isProvoked = false;
            _timer = 0;
        }

        if (!IsInChaseRange() && !_isProvoked)
        {
            StateMachine.SwitchState(new EnemyIdleState(StateMachine, true));
            return;
        }
        if (!IsInChaseRange() && _isProvoked)
        {
            Chase(deltaTime);
            return;
        }
        
        if (IsInAttackRange() && StateMachine.IsMelee)
        {
            StateMachine.SwitchState(new EnemyAttackingState(StateMachine));
            return;
        }
        
        if (IsInAttackRange() && !StateMachine.IsMelee)
        {
            StateMachine.transform.LookAt(StateMachine.Player.transform);
            StateMachine.SwitchState(new EnemyRangedAttack(StateMachine));
            return;
        }
        
        Chase(deltaTime);

        
    }


    public override void Exit()
    {
        StateMachine.Agent.isStopped = true;
        StateMachine.isAlarmed = false;
    }

    private void HandlePlayerDeath()
    {
        StateMachine.Agent.enabled = false;
        StateMachine.Animator.CrossFadeInFixedTime(_locomotionHash, 0.2f);
        StateMachine.Animator.SetFloat(_speedHash, 0f);
        StateMachine.enabled = false;
    }

    private bool IsInAttackRange() =>
        Vector3.Distance(StateMachine.Player.transform.position, StateMachine.transform.position) <=
        StateMachine.AttackRange;
    
    private void Chase(float deltaTime)
    {
        StateMachine.Animator.SetFloat(_speedHash, 1f, AnimatorDampTime, deltaTime);
        StateMachine.Agent.SetDestination(StateMachine.Player.transform.position);
    }

}