using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState() : 
        base("Idle State")
    {

    }

    public override void StateEnter(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isIdle", true);
    }

    public override void StateUpdate(FSMPlayerBehavior p)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        
        
        if(p.plrScr.ProcessaInputAttacco())
        {
            p.SwitchState(p.playerAttackState);
        }

        if(p.plrScr.ProcessaInputSchivata())
        {
            p.SwitchState(p.playerDodgeState);
        }

        if(p.plrScr.ProcessaInputGrab())
        {
            p.SwitchState(p.playerGrabState);
        }

        // Se si muove
        if(x != 0 || z != 0)
        {
            p.SwitchState(p.playerRunState);
        }


    }
    public override void StateExit(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isIdle", false);  
    }

    public override void ProcessTriggers(FSMPlayerBehavior p)
    {
        ProcessBarrelsTriggers(p);
        ProcessEnemyTriggers(p);
    }
}
