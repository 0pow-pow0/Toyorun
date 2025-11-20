using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

public class PlayerDodgeState : PlayerBaseState
{
    Timer dodgeDuration;
    Timer dodgeCooldown;
    int DODGE_FORCE = 150; 

    public PlayerDodgeState() :
        base("Dodge State")
    {
        dodgeDuration = new Timer(0.4f);
        dodgeCooldown = new Timer(1f);
    }
    public override bool CanEnterState(FSMPlayerBehavior p)
    {        
        if(!dodgeCooldown.HasEnded())
        {
            Debug.Log("Dodge in cooldown");
            return false;
         }

        return true;
    }
    public override void StateEnter(FSMPlayerBehavior p)
    {
        Player plr = p.plr.GetComponent<Player>();
        plr.rb.velocity = Vector3.zero;
        plr.rb.AddForce(plr.transform.forward * DODGE_FORCE, ForceMode.VelocityChange);

        plr.anim.SetBool("isDodge", true);
    }

    public override void StateUpdate(FSMPlayerBehavior p)
    {
        if (dodgeDuration.HasEnded())
        {
            dodgeDuration.Restart();
            p.SwitchState(p.playerIdleState);
        }
        else
        {
            dodgeDuration.UpdateTime();
        }
    }
    public override void StateExit(FSMPlayerBehavior p)
    {
        if(dodgeCooldown.HasEnded()) { dodgeCooldown.Restart(); }
        p.plr.GetComponent<Player>().rb.velocity = Vector3.zero;
        p.plr.GetComponent<Player>().anim.SetBool("isDodge", false);
    }

    public override void AnyStateUpdate(FSMPlayerBehavior p)
    {
        if(!dodgeCooldown.HasEnded())
        {
            dodgeCooldown.UpdateTime();
        }
        //Debug.Log("vel" + p.plr.GetComponent<Player>().rb.velocity);
    }
}
