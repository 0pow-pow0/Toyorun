using EntitiesCostants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;


// Non si puo' transitare in questo stato tramite una collisione 
// degli attacchi del nemico SE ci si trova in QUESTO stato.
public class SimpleEnemyHurtState : SimpleEnemyBaseState
{
    Timer hurtTime; // Quanto tempo rimane in hurtState? 
    
    // Questo valore mi serve per sapere quale attacco di una combo mi ha colpito.
    // Grazie a questa variabile posso evitare che lo stesso attacco
    // richiami piu' volte il codice del danno, ad esempio.
    // Visto che OnTriggerEnter non e' utilizzabile per il problema spiegato in Player.CS
    // Resettare ad uno stato invalido prima di uscire dallo stato
    uint indexChainOfReceivedAttack = 9999;

    public SimpleEnemyHurtState(FSMSimpleEnemyBehavior p) :
        base("Hurt State")
    {
        hurtTime = new Timer(1.5f);
    }

    public override bool CanEnterState(FSMSimpleEnemyBehavior p)
    {        
        // Non si puo' diventare feriti se si e' stati RIcolpiti dallo stesso attacco
        if(indexChainOfReceivedAttack == Player.activeCombo.indexChain)
        {
            return false;
        }

        return true;
    }
    public override void StateEnter(FSMSimpleEnemyBehavior p)
    {
        // Devo necessariamente stoppare la coroutine di prima.
        // ALtrimenti certe volte potrebbe non mettersi in code la routine startata.
        //p.enemScr.StopCoroutine("BlendOutlineTo");
        p.enemScr.outlineScr.SetColor(
            SimpleEnemyCostants.instance().COLOR_OUTLINE_JUST_HITTED);
        p.enemScr.outlineScr.BlendOulineColorTo(
            SimpleEnemyCostants.instance().COLOR_OUTLINE_HURT, 0.7f);

        indexChainOfReceivedAttack = Player.activeCombo.indexChain;
        Debug.Log("HP BEFORE: " + p.enemScr.hp);
        p.enemScr.hp -= Player.activeCombo.GetActiveAttacco().danno;
        Debug.Log("HP AFTER: " + p.enemScr.hp);
        p.enemScr.anim.SetBool("isHurt", true);
        p.enemScr.rb.velocity = Vector3.zero;
        hurtTime.Restart();
        p.enemScr.rb.AddRelativeForce(new Vector3(0, 0, 40));
    }

    public override void StateUpdate(FSMSimpleEnemyBehavior p)
    {
        if(hurtTime.HasEnded())
        {
            p.SwitchState(p.simpleEnemyIdleState);
        }
        else
        {
            hurtTime.UpdateTime();
        }
        Debug.Log("HP: " + p.enemScr.hp);
    }

    public override void StateExit(FSMSimpleEnemyBehavior p)
    {
        p.enemScr.anim.SetBool("isHurt", false);
        p.enemScr.rb.velocity = Vector3.zero;
        // Metto ad uno stato invalido
        indexChainOfReceivedAttack = 9999;
        p.enemScr.outlineScr.BlendOulineColorTo(SimpleEnemyCostants.instance().COLOR_OUTLINE_BASE, 0.4f);
    }

    public override void AnyStateUpdate(FSMSimpleEnemyBehavior p)
    {

    }

    // Non si puo' transitare in questo stato tramite una collisione 
    // degli attacchi del nemico SE ci si trova in QUESTO stato.
    public override void CheckCollisions(FSMSimpleEnemyBehavior p)
    {
        // Se siamo dentro lo stesso collider che ci ha gia' danneggiati, evitiamo che risucceda
        if(indexChainOfReceivedAttack == Player.activeCombo.indexChain) { return; }


        // Se collidiamo con gli attacchi del player
        if (
            PowUtility.CheckBox(p.enemScr.bodyCollider,
            LayerMaskCostants.instance().playerAttack)
        )
        {
            Debug.Log("INSIDE");
            p.enemScr.outlineScr.SetColor(
                SimpleEnemyCostants.instance().COLOR_OUTLINE_JUST_HITTED);
            p.enemScr.outlineScr.BlendOulineColorTo(
                SimpleEnemyCostants.instance().COLOR_OUTLINE_HURT, 0.7f);

            hurtTime.Restart();
            p.enemScr.hp -= Player.activeCombo.GetActiveAttacco().danno;
            indexChainOfReceivedAttack = Player.activeCombo.indexChain;
        }
    }
}