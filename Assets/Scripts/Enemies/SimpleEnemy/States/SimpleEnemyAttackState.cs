using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;
using EntitiesCostants;

/// <summary>
/// Ogni attacco del nemico ha due fasi:
/// una in cui carica l'attacco e vengono mostrati al player degli indizi
/// grafici che fanno capire quanto manchi per attaccare, 
/// e poi l'attacco vero e proprio.
/// </summary>
public class SimpleEnemyAttackState : SimpleEnemyBaseState
{
    public Timer attackChargeTimer; // Timer che regola il tempo di carica dell'attacco
    Timer attackDuration;              // Timer che regola la durata dell'attacco dopo la carica

    // Non so se e' utile visto che i timer degli altri stati gia' gestiscono questa roba
    Timer attackCooldown;             // Timer che regola il tempo necessario per riattaccare

    // Sarebbe il piano che viene scalato per far capire al player quanto
    // manca al nemico prima che attacchi
    // [Presi da SimpleEnemy.cs]
    //public GameObject pivotSliderAttackHint;
    //public GameObject pivtSliderAttackHintBackground;

    public SimpleEnemyAttackState(FSMSimpleEnemyBehavior p) :
        base("Attack State")
    {
        //---------------------------- TIMERS INITS
        attackChargeTimer = new Timer(0.8f);
        attackDuration   = new Timer(1.5f);
        attackCooldown   = new Timer(2f);
        attackCooldown.End(); // Ovviamente non sara' in cooldown all'inizio           

        
        p.enemScr.attackHintSliderParent.transform.localScale = new Vector3(1, 1, 1); // Inizializzo cosi' non possono esserci errori
        p.enemScr.attackHintBackground.SetActive(false);
        p.enemScr.attackHintSliderParent.SetActive(false);
    }

    public override bool CanEnterState(FSMSimpleEnemyBehavior p)
    {
        // Se e' in cooldown ancora non far cambiare di stato
        if(!attackCooldown.HasEnded()) 
        {
            return false;
        }
        return true;
    }

    public override void StateEnter(FSMSimpleEnemyBehavior p)
    {
        // Reinizializzare le variabili fondamentali
        attackDuration.Restart();
        attackChargeTimer.Restart();
        //Debug.Log("ATTACK INIT");
        // Resetta barra che mostra timing attacco
        p.enemScr.attackHintSliderParent.transform.localScale = new Vector3(1, 1, 0);
        // Attiva i piani che mostrano il timing
        p.enemScr.attackHintSliderParent.SetActive(true);
        p.enemScr.attackHintBackground.SetActive(true);

        p.enemScr.anim.SetBool("isCharge", true);
    }
    
    public override void StateUpdate(FSMSimpleEnemyBehavior p)
    {
        if(!attackChargeTimer.HasEnded())
        {
            // Semplce formula che scala in base il piano in base al tempo che impiega
            // l'attacco a completare il caricamento
            p.enemScr.attackHintSliderParent.transform.localScale = new Vector3(1, 1, 
                attackChargeTimer.elapsedTime / attackChargeTimer.maxTime);
            attackChargeTimer.UpdateTime();
        }
        // Appena il timer e' finito
        else
        {
            // Eh si un po' di calcoli buttati :)
            p.enemScr.attackCollider.enabled = true;
            p.enemScr.anim.SetBool("isCharge", false);
            p.enemScr.anim.SetBool("isAttack", true);
            // TODO ENABLE COLLISIONS
            // TODO ATTIVA PARTICELLARE
            attackDuration.UpdateTime();
            if(attackDuration.HasEnded())
            {
                p.enemScr.attackCollider.enabled = false;
                p.SwitchState(p.simpleEnemyWalkingToState);
            }
        }
    }

    public override void StateExit(FSMSimpleEnemyBehavior p)
    {
        // Resettare sempre quello che si modifica in update
        p.enemScr.anim.SetBool("isCharge", false);
        p.enemScr.anim.SetBool("isAttack", false);
        p.enemScr.attackCollider.enabled = false;
        if(attackCooldown.HasEnded())
        {
            attackCooldown.Restart();
        }

        p.enemScr.attackHintSliderParent.SetActive(false);
        p.enemScr.attackHintBackground.SetActive(false);
    }

    // Non posso essere colpito!!!!!! 
    //public override void CheckCollisions(FSMSimpleEnemyBehavior p)
    //{
    //    // No codice perche' non puedo essere colpito

    //    // TODO particellaris

    //    CheckCollisionsWithPlayerAttacks(p);
    //    // Se prova a grabbarmi in questo stato viene stunnato xddd
    //    // Solo se non ha gia' grabbato qualcuno 
    //    // Lo rimuove, limita troppo il gameplay
    //    //if(Player.FSM.playerGrabState.hasGrabbedSomeone && 
    //    //    PowUtility.CheckBox(p.enemScr.bodyCollider, LayerMaskCostants.instance().playerGrab))
    //    //{
    //    //    Player.FSM.SwitchState(Player.FSM.playerStunnedState);
    //    //}

    //}

    public override void AnyStateUpdate(FSMSimpleEnemyBehavior p)
    {
        if(!attackCooldown.HasEnded())
        {
            attackCooldown.UpdateTime();
        }
    }
}