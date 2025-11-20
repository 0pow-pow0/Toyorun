using EntitiesCostants;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;
/// <summary>
/// Stessa cosa di PlayerBaseState.cs
/// </summary>
public abstract class MageEnemyBaseState
{
    public string _d_stateName = "lollo";
    public bool isActive = false;

    public MageEnemyBaseState(string stateName)
    {
        _d_stateName = stateName;
    }

    public virtual bool CanEnterState(FSMMageEnemyBehaviour p)
    {
        return true;
    }

    public abstract void StateEnter(FSMMageEnemyBehaviour p);
    public abstract void StateUpdate(FSMMageEnemyBehaviour p);
    public abstract void StateExit(FSMMageEnemyBehaviour p);

    public virtual void OnTriggerEnter(FSMMageEnemyBehaviour p, Collider other)
    {
        //Debug.Log("PALLE");
    }


    public virtual void CheckCollisions(FSMMageEnemyBehaviour p)
    {
        if(p.enemScr.isImmortal) {  return; }

        //sDebug.Log(p.enemScr.bodyCollider.transform.position + " " + p.enemScr.bodyCollider.size);
        Collider[] colls = PowUtility.OverlapBox(
            p.enemScr.bodyCollider,
            LayerMaskCostants.instance().simpleEnemiesBody);
        //Debug.Log(colls.Length);
        if(colls.Length > 0)
        {
            Debug.Log(colls[0].excludeLayers.ToString() + " " + colls[0].name);
            GameObject enemy = colls[0].gameObject.transform.parent.transform.parent.gameObject;
            FSMSimpleEnemyBehavior enemyFSM = enemy.GetComponent<FSMSimpleEnemyBehavior>();

            // Se sta venendo lanciato vieni colpito
            if(enemyFSM.simpleEnemyThrownState.isActive)
            {
                // fai del male anche al tizio lanciato
                enemyFSM.SwitchState(enemyFSM.simpleEnemyHurtState);
                p.SwitchState(p.mageEnemyHurtState);
            }
        }
    }
}
