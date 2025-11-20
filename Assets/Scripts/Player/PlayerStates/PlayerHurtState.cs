using EntitiesCostants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;


/// <summary>
/// Classica Finite State Machine
/// Creiamo un'interfaccia virtuale che descrive 
/// dei metodi comuni a tutti gli stati possibili del player
/// C'e' un piccolo twist, ovvero la funzione AnyState
/// Che verra' eseguita INDIPENDENTEMENTE DALLO STATO ATTUALE
/// Usare con parsimonia, grz   
/// 
/// </summary>
public class PlayerHurtState : PlayerBaseState
{
    Timer hurtStateTime;    // Tempo in cui devo rimanere in hurt state
    Timer invincibilityTime;  // Tempo in cui e' invincibile
    public int attackDamage;
    public PlayerHurtState() :
        base("Hurt State")
    {
        hurtStateTime = new Timer(
            PlayerCostants.instance().HURT_DURATION_TIME);
        invincibilityTime = new Timer(
            PlayerCostants.instance().HURT_INVINCIBILITY_DURATION_TIME);
    }

    public override bool CanEnterState(FSMPlayerBehavior p)
    {        
        // Se sono invincibile non posso ridiventare hurt
        if (p.plrScr.isInvincible) { return false; }

        return true;
    }
    public override void StateEnter(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isHurt", true);
        invincibilityTime.Restart();
        hurtStateTime.Restart();

        // Pev Benisti come l'eclissi
        for (int i = 0; i < attackDamage; i++)
        {
            p.plrScr.LoseHp();
        }
        p.plrScr.isInvincible = true;
        p.plrScr.outlineScr.SetColor(PlayerCostants.instance().outlineColorIinvincible);
    }

    public override void StateUpdate(FSMPlayerBehavior p)
    {
        if(!hurtStateTime.HasEnded())
        {
            hurtStateTime.UpdateTime();
        }
        else
        {
            p.SwitchState(p.playerIdleState);
        }
    }

    public override void StateExit(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isHurt", false);
    }

    public override void AnyStateUpdate(FSMPlayerBehavior p)
    {
        if(p.plrScr.isInvincible)
        {
            if (!invincibilityTime.HasEnded())
            {
                invincibilityTime.UpdateTime();
            }
            else
            {
                // Lo so, si settera' ogni frame, ma siccome niente a parte questo file
                // puo' modificare questa variabile, sti cazzi
                p.plrScr.isInvincible = false;
                p.plrScr.outlineScr.SetColor(PlayerCostants.instance().outlineColorBase);
            }
        }

    }

    //public override void OnTriggerEnter(FSMPlayerBehavior p, Collider other)
    //{
        // Sono gia' in hurt state, non posso essere colpito
        // MA posso collezionare gli oggetti
    //}
}
