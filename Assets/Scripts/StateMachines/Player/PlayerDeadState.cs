using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadState : PlayerBaseState
{
    private const string DeadAnimationName = "PlayerDeathAnimation";
    private const float WAIT_TIME = 1f;
    private float _timer;

    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StateMachine.PlayerThirdPersonCameraController.canAim = false;
        StateMachine.Animator.CrossFadeInFixedTime(DeadAnimationName, DEFAULT_CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime)
    {
        _timer += deltaTime;
        Move(deltaTime);

        if (_timer >= WAIT_TIME)
        {
            SceneManager.LoadScene(0);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public override void Exit()
    {
    }
}