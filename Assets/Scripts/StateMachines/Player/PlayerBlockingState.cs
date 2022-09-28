using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int _blockAnimationHash = Animator.StringToHash("Block");
    private float _timer;
    private const float STAMINA_REDUCING_INTERVAL = 1f;
    private Collider _swordCollider;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _swordCollider = StateMachine.Sword.GetComponent<Collider>();
        _swordCollider.enabled = false;
        
        StateMachine.Health.isInvulnerable = true;
        StateMachine.PlayerStamina.CanRestore = false;
        StateMachine.Animator.CrossFadeInFixedTime(_blockAnimationHash, DEFAULT_CROSS_FADE_DURATION);
        StateMachine.PlayerThirdPersonCameraController.canAim = false;
        StateMachine.SetMeleeGameObjectsActive(true);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (_timer >= STAMINA_REDUCING_INTERVAL)
        {
            StateMachine.PlayerStamina.ReduceStamina(StateMachine.BlockingStaminaCost);
            _timer = 0f;
        }

        var hasEnoughStamina = StateMachine.PlayerStamina.CurrentStamina >= StateMachine.BlockingStaminaCost;
        if (!StateMachine.InputReader.IsBlocking || !hasEnoughStamina)
        {
            StateMachine.Health.isInvulnerable = false;
            StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
            return;
        }

        if (StateMachine.Targeter.CurrentTarget == null)
        {
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            return;
        }

        _timer += deltaTime;
    }

    public override void Exit()
    {
        StateMachine.Health.isInvulnerable = false;
        StateMachine.PlayerStamina.CanRestore = true;
        if (StateMachine.ShouldHideSwordInFreeLookState) StateMachine.SetMeleeGameObjectsActive(false);
    }
}