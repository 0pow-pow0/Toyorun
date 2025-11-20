using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

/// <summary>
/// Praticamente quando e' in questo stato non deve fare un cazzo xd.
/// Resetta la posizione del NavMeshAgent
/// </summary>
public class SimpleEnemyIdleState : SimpleEnemyBaseState
{
    Timer timeToSwitch;

    public SimpleEnemyIdleState(FSMSimpleEnemyBehavior p) :
        base("Idle State")
    {
        timeToSwitch = new Timer(0f);
    }

    public override void StateEnter(FSMSimpleEnemyBehavior p)
    {
        p.enemScr.anim.SetBool("isIdle", true);
        timeToSwitch.ChangeMaxTime(Random.Range(0.5f, 4f));
        timeToSwitch.Restart();
    }

    public override void StateUpdate(FSMSimpleEnemyBehavior p)
    {
        if(!timeToSwitch.HasEnded())
        {
            timeToSwitch.UpdateTime();
        } 
        else
        {
            p.SwitchState(p.simpleEnemyWalkingToState);
        }

    }

    public override void StateExit(FSMSimpleEnemyBehavior p)
    {
        p.enemScr.anim.SetBool("isIdle", false);
    }

    public override void AnyStateUpdate(FSMSimpleEnemyBehavior p)
    {

    }
}