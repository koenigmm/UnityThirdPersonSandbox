using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyBaseState : State
{

    protected EnemyStateMachine StateMachine;
    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    
    public float FindAnimationClipLength(string animationClipName)
    {
        return (from animationClip in StateMachine.Animator.runtimeAnimatorController.animationClips
            where animationClip.name == animationClipName 
            select animationClip.length).FirstOrDefault();
    }
    

}
