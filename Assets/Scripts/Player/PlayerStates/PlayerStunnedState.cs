using EntitiesCostants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

/// <summary>
/// Se misso il grab e prendo un nemico ingrabbabile, mi stunno xd
/// </summary>
public class PlayerStunnedState : PlayerBaseState
{
    Timer stunTime; // Tempo in cui rimane stunnato
    Timer knockbackTime; // Tempo in cui si applica il knockback

    public PlayerStunnedState(FSMPlayerBehavior p) :
        base("Stunned State")
    {
        stunTime = new Timer(
            PlayerCostants.instance().STUNNED_DURATION_TIME);

        knockbackTime = new Timer(
            PlayerCostants.instance().STUNNED_KNOCKBACK_DURATION_TIME);
    }

    public override void StateEnter(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isStun", true);

        // Aggiungi piccolo knockback

        p.plrScr.rb.AddRelativeForce(
            PlayerCostants.instance().STUNNED_KNOCKBACK_FORCE,
            ForceMode.Impulse);

        stunTime.Restart();
        knockbackTime.Restart();    
    }
    public override void StateUpdate(FSMPlayerBehavior p)
    {
        if(stunTime.HasEnded())
        {
            p.SwitchState(p.playerIdleState);
        }

        knockbackTime.UpdateTime();
        stunTime.UpdateTime();
    }

    public override void StateExit(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isStun", false);

        // Resetto anche se non e' strettamente necessario per evitare
        // accumuli di velocita'
        p.plrScr.rb.velocity = Vector3.zero;
    }

    // Puo' essere colpito e scopato
}
