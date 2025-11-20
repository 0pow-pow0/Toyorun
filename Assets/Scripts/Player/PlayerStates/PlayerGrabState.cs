using EntitiesCostants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

/// <summary>
/// Attiva collider di grab e se il nemico viene grabbato ancoralo al grabPivot del player
/// </summary>
public class PlayerGrabState : PlayerBaseState
{
    // Variabile che serve per comunicare ai nemici se qualcuno e' stato grabbato effettivaemente
    public bool hasGrabbedSomeone = false;

    // Tempo prima che il grab si annulli da solo se non prendi nessuno
    Timer timeBeforeBreakUnsuccessfulGrab;
    // // se prendi un nemico, puoi tenerlo in mano per un po' di tempo
    Timer timeBeforeBreakSuccesfulGrab; 

    //Timer cooldown;

    public PlayerGrabState(FSMPlayerBehavior p) :
        base("Grab State")
    {
        timeBeforeBreakUnsuccessfulGrab = new Timer(PlayerCostants.instance().GRAB_ANIMATION_DURATION);
        timeBeforeBreakSuccesfulGrab = new Timer(PlayerCostants.instance().GRAB_MAX_GRAB_TIME_DURATION);
    }

    public override void StateEnter(FSMPlayerBehavior p)
    {
        // TODO sistema qui e l'animator perche' non serve avere un bool e un trigger
        // mi rompo il cazzo a sistermare xd
        p.plrScr.anim.SetTrigger("isGrab 0");
        p.plrScr.anim.SetBool("isGrab", true);
        p.plrScr.grabCollider.enabled = true;
        timeBeforeBreakUnsuccessfulGrab.Restart();
        timeBeforeBreakSuccesfulGrab.Restart();
        // Viene settato al momento della collisione dal nemico che ha colliso con il grabCollider
        //hasGrabbedSomeone = true; 
     
    }
    public override void StateUpdate(FSMPlayerBehavior p)
    {
        // Praticamente ho copiato il codice di playerRunState
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Se non sta camminando
        if (x == 0f && z == 0f)
        {
            p.plrScr.anim.SetBool("isGrabStill", true);
            p.plrScr.anim.SetBool("isGrabRun", false);
        } 
        // Se sta camminando
        else
        {
            p.plrScr.anim.SetBool("isGrabStill", false);
            p.plrScr.anim.SetBool("isGrabRun", true);
        }
        p.plrScr.rb.velocity = new Vector3(x * p.plrScr.speed, 0, z * p.plrScr.speed);
        p.plrScr.AggiustaRotazione(x, z);





        // -------------------------- TIMERS
        timeBeforeBreakUnsuccessfulGrab.UpdateTime();
        timeBeforeBreakSuccesfulGrab.UpdateTime();

        // Se non ha grabbato nuddu
        if(!hasGrabbedSomeone)
        {
            if(timeBeforeBreakUnsuccessfulGrab.HasEnded())
            {
                p.SwitchState(p.playerIdleState);
                return;
            }
        }
        // Se ha grabbato qualcuno
        else 
        {        
            // -------------------------- INPUT LANCIO
            if(p.plrScr.ProcessaInputLancio())
            {
                p.SwitchState(p.playerThrowState);
                return;
            }

            if(timeBeforeBreakSuccesfulGrab.HasEnded())
            {
                p.SwitchState(p.playerIdleState);
                return;
            }
        }
    }

    public override void StateExit(FSMPlayerBehavior p)
    {
        p.plrScr.grabCollider.enabled = false;
        p.plrScr.rb.velocity = Vector3.zero;
        p.plrScr.anim.SetBool("isGrab", false);
        p.plrScr.anim.SetBool("isGrabRun", false);
        p.plrScr.anim.SetBool("isGrabStill", false);
        hasGrabbedSomeone = false; 
    }
}
