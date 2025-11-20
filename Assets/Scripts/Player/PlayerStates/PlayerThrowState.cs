using EntitiesCostants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

public class PlayerThrowState : PlayerBaseState
{
    Timer stillTime; // Tempo in cui rimane fermo

    public PlayerThrowState(FSMPlayerBehavior p) :
        base("Throw State")
    {
        stillTime = new Timer(PlayerCostants.instance().THROW_STILL_DURATION);
    }

    public override void StateEnter(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isThrow", true);
        stillTime.Restart();
    }
    public override void StateUpdate(FSMPlayerBehavior p)
    {
        if (stillTime.HasEnded())
        {
            p.SwitchState(p.playerIdleState);
        }

        stillTime.UpdateTime();
    }

    public override void StateExit(FSMPlayerBehavior p)
    {
        p.playerGrabState.hasGrabbedSomeone = false;
        p.plrScr.anim.SetBool("isThrow", false);
    }
}
