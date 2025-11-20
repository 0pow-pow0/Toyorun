using EntitiesCostants;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UtilityShit;

/// <summary>
/// Se misso il grab e prendo un nemico ingrabbabile, mi stunno xd
/// </summary>
public class PlayerMoveObjectState : PlayerBaseState
{
    public Barrel barrelScr; 
    private GameObject pivot;

    public PlayerMoveObjectState(FSMPlayerBehavior p) :
        base("Move Object State")
    {
    }
    GameObject GetNearesPivot(FSMPlayerBehavior p)
    {
        // Prendi il pivot piu' vicino
        GameObject nearest = null;
        float minDist = 9999999f;
        for (int i = 0; i < barrelScr.pivots.Length; i++)
        {
            float d = Vector3.Distance(
                barrelScr.pivots[i].transform.position,
                p.plrScr.transform.position);
            if (d < minDist)
            {
                nearest = barrelScr.pivots[i];
                minDist = d;
            }
        }
        // Non si sa mai
        if (minDist == 9999999) { Debug.LogError("Errore!"); return null; }
        return nearest;
    }

    public override bool CanEnterState(FSMPlayerBehavior p)
    {
        return Input.GetKey(KeyCode.U);//p.plrScr.ProcessaInputAttacco();
    }

    public override void StateEnter(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isMoveObject", true);
        p.plrScr.anim.SetBool("isMoveObjectStill", true);

        Debug.Log("INSIDE");

        pivot = GetNearesPivot(p);
    }
    public override void StateUpdate(FSMPlayerBehavior p)
    {
        // Se smette di premere il tastino xd
        if(!Input.GetKey(KeyCode.U))
        {
            p.SwitchState(p.playerIdleState);
            return;
        }


        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Non vogliamo modificare la Y del player per evitare di sminchiare la gravita'
        // Sappiamo tutti cose potrebbe succedere
        Vector3 piv = new Vector3(
            pivot.transform.position.x, 
            p.plrScr.transform.position.y,
            pivot.transform.position.z);
        
        // Muovi verso il pivot
        p.plrScr.transform.position = Vector3.MoveTowards(
            p.plrScr.transform.position,
            piv,
            Time.deltaTime * 50
            );
        // Ruota verso il pivot
        p.plrScr.transform.localRotation =
            Quaternion.RotateTowards(
                p.plrScr.transform.localRotation,
                Quaternion.LookRotation(pivot.transform.forward),
                Time.deltaTime * 500
                );

        //p.plrScr.transform.position

        //Debug.Log("For:" + pivot.transform.forward);
        // Se e' fermo
        if(x == 0 && z == 0)
        {
            p.plrScr.anim.SetBool("isMoveObjectStill", true);
            p.plrScr.anim.SetBool("isMoveObjectMove", false);

            //p.plrScr.rb.velocity = Vector3.zero;
            barrelScr.rb.velocity = Vector3.zero;
        }
        // Se si muove
        else
        {
            p.plrScr.anim.SetBool("isMoveObjectStill", false);
            p.plrScr.anim.SetBool("isMoveObjectMove", true);

            Debug.Log(pivot.transform.forward);
            /// Presenta dei problemi, ma va bene cosi', non si puo' ruotare il barile ad esempio,
            /// non si puo' muovere di lato
            // Moltiplichiamo i versi per ottenere quello corretto relativo al pivot e all'input
            Vector3 newShit = pivot.transform.forward * PlayerCostants.instance().MOVE_STATE_SPEED;
            newShit = new Vector3(
                newShit.x * (x * Math.Sign(pivot.transform.forward.x)),
                0,
                newShit.z * (z * Math.Sign(pivot.transform.forward.z)));

            Debug.Log(newShit);

            //Debug.Log("vel dir: " + inputDirectionAdjusted); 
            barrelScr.rb.velocity = newShit;
        }
    }

    public override void StateExit(FSMPlayerBehavior p)
    {
        barrelScr.rb.velocity = Vector3.zero;
        p.plrScr.anim.SetBool("isMoveObjectStill", false);
        p.plrScr.anim.SetBool("isMoveObjectMove", false);
        p.plrScr.anim.SetBool("isMoveObject", false);
    }

    public override void ProcessTriggers(FSMPlayerBehavior p)
    {
        // nullal
    }

}
