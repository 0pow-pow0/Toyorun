using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntitiesCostants;
using UtilityShit;


/// <summary>
/// Classica Finite State Machine
/// Creiamo un'interfaccia virtuale che descrive 
/// dei metodi comuni a tutti gli stati possibili del player
/// C'e' un piccolo twist, ovvero la funzione AnyState
/// Che verra' eseguita INDIPENDENTEMENTE DALLO STATO ATTUALE
/// Usare con parsimonia, grz   
/// </summary>
public abstract class PlayerBaseState
{
    public string _d_stateName;
    public bool isActive = false;
    public PlayerBaseState(string str)
    {
        _d_stateName = str;
    }

    public virtual bool CanEnterState(FSMPlayerBehavior p)
    {
        return true;
    }
    public abstract void StateEnter(FSMPlayerBehavior p);
    public abstract void StateUpdate(FSMPlayerBehavior p);
    public abstract void StateExit(FSMPlayerBehavior p);

    // Usare con molta calma dato che verra' eseguita ogni volta
    // Non dovrebbero essere chiamate direttamente,
    // ma salvate all'interno di un puntatore che le eseguira'
    // in tal modo si puo' appunto avere un INDIPENDENZA dagli altri stati
    public virtual void AnyStateUpdate(FSMPlayerBehavior p)
    {

    }

    // Sono funzioni che evitano che io ricopi negli stati in cui overrido OnTriggerEnter.
    public void ProcessBarrelsTriggers(FSMPlayerBehavior p)
    {
        //Debug.Log("1");
        Collider[] colls = PowUtility.OverlapBox(p.plrScr.bodyCollider,
            LayerMaskCostants.instance().barrelsActivationRange);

        if(colls.Length > 0)
        {
            Debug.Log("COLLISION");
            Barrel barrel = colls[0].transform.parent.gameObject.GetComponent<Barrel>();
            p.playerMoveObjectState.barrelScr = barrel;
            p.SwitchState(p.playerMoveObjectState);
        }
    }

    public void ProcessEnemyTriggers(FSMPlayerBehavior p)
    {
        if (p.plrScr.isInvincible) { return; }

        //Debug.Log(p.plrScr.transform.position + " " + p.plrScr.bodyCollider.center);
        if (
           PowUtility.CheckBox(p.plrScr.bodyCollider, 
           LayerMaskCostants.instance().simpleEnemiesAttack
           )
       )
        {
            p.playerHurtState.attackDamage = SimpleEnemyCostants.instance().ATTACK;
            p.SwitchState(p.playerHurtState);
        }

        if(
            PowUtility.CheckBox(p.plrScr.bodyCollider,
            LayerMaskCostants.instance().mageEnemiesSphereAttack
                ))
        {
            p.playerHurtState.attackDamage = MageEnemyCostants.instance().SPHERE_ATTACK_DAMAGE;
            p.SwitchState(p.playerHurtState);
        }
    }

    public void ProcessCollectiblesTriggers(FSMPlayerBehavior p)
    {

    }

    public virtual void ProcessTriggers(FSMPlayerBehavior p)
    {
        ProcessEnemyTriggers(p);
        ProcessCollectiblesTriggers(p);
    }
}
  