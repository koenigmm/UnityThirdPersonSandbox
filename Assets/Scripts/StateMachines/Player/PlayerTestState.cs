using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    private float _timer;
    
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enter");
        StateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Tick(float deltaTime)
    {
        _timer += deltaTime;
        Debug.Log($"RemainingTime: {_timer}");
    }

    public override void Exit()
    {
        Debug.Log("Exit");
        StateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump()
    {
        StateMachine.SwitchState(new PlayerTestState(StateMachine));
    }
}