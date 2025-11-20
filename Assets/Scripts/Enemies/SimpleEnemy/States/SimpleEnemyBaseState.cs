using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;
/// <summary>
/// Stessa cosa di PlayerBaseState.cs
/// </summary>

public abstract class SimpleEnemyBaseState
{
    public string _d_stateName = "lollo";
    public bool isActive = false;

    public SimpleEnemyBaseState(string stateName)
    {
        _d_stateName = stateName;
    }

    // Serve per verificare se ci sono le condizioni per cambiare stato
    public virtual bool CanEnterState(FSMSimpleEnemyBehavior p)
    {
        return true;
    }
    public abstract void StateEnter(FSMSimpleEnemyBehavior p);
    public abstract void StateUpdate(FSMSimpleEnemyBehavior p);
    public abstract void StateExit(FSMSimpleEnemyBehavior p);


    public virtual void AnyStateUpdate(FSMSimpleEnemyBehavior p)
    {

    }
    public virtual void OnTriggerEnter(FSMSimpleEnemyBehavior p, Collider other)
    {
        //Debug.Log("PALLE");
    }

    // Controlla se il bodycollider ha colliso con qualche attacco del player
    public void CheckCollisionsWithPlayerAttacks(FSMSimpleEnemyBehavior p)
    {
        //Debug.Log(p.enemScr.bodyCollider.transform.position);
        if (
        PowUtility.CheckBox(p.enemScr.bodyCollider, LayerMask.GetMask("PlayerAttackCollider")))
        {
            p.SwitchState(p.simpleEnemyHurtState);
        }
    }

    public void CheckTriggerWithPlayerGrab(FSMSimpleEnemyBehavior p)
    {
        
        if (//!Player.FSM.playerGrabState.hasGrabbedSomeone && 
            PowUtility.CheckBox(p.enemScr.bodyCollider, LayerMask.GetMask("PlayerGrabCollider")))
        {
            Player.FSM.playerGrabState.hasGrabbedSomeone = true;
            p.SwitchState(p.simpleEnemyGrabbedState);
        }
    }


    public virtual void CheckCollisions(FSMSimpleEnemyBehavior p)
    {
        CheckCollisionsWithPlayerAttacks(p);
        CheckTriggerWithPlayerGrab(p);
    }
}
