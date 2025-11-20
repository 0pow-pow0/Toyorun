using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

public class PlayerRunState : PlayerBaseState 
{
    public PlayerRunState() : 
        base("Run State")
    {

    }
    public override void StateEnter(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isRun", true);
    }

    public override void StateUpdate(FSMPlayerBehavior p)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");



        p.plrScr.rb.velocity = new Vector3(x * p.plrScr.speed, p.plrScr.rb.velocity.y, z * p.plrScr.speed);

        //-------------------- Transizioni
        #region TRANSIZIONI
        if(p.plrScr.ProcessaInputAttacco())
        {
            p.SwitchState(p.playerAttackState);
        }

        if (p.plrScr.ProcessaInputSchivata())
        {
            p.SwitchState(p.playerDodgeState);
        }

        if (p.plrScr.ProcessaInputGrab())
        {
            p.SwitchState(p.playerGrabState);
        }

        if (x == 0 && z == 0)
        {
            p.SwitchState(p.playerIdleState);
        }
        #endregion
        p.plrScr.AggiustaRotazione(x, z);
    }

    public override void StateExit(FSMPlayerBehavior p)
    {
        p.plrScr.rb.velocity = Vector3.zero;
        p.plrScr.anim.SetBool("isRun", false);
    }

    public override void ProcessTriggers(FSMPlayerBehavior p)
    {
        ProcessBarrelsTriggers(p);
        ProcessEnemyTriggers(p);
    }

}
