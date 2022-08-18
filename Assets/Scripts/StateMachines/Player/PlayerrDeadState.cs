﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerrDeadState : PlayerBaseState
{
    private const string DeadAnimationName = "PlayerDeathAnimation";
    private const float WAIT_TIME = 2f;
    private float _timer;

    public PlayerrDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter() =>
        StateMachine.Animator.CrossFadeInFixedTime(DeadAnimationName, DEFAULT_CROSS_FADE_DURATION);

    public override void Tick(float deltaTime)
    {
        _timer += deltaTime;
        Move(deltaTime);

        if (_timer >= WAIT_TIME)
            // TODO Show UI and load a savegame
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void Exit()
    {
    }
}