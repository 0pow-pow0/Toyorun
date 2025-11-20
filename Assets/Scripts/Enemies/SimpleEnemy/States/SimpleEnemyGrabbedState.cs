using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SimpleEnemyGrabbedState : SimpleEnemyBaseState
{
    


    public SimpleEnemyGrabbedState(FSMSimpleEnemyBehavior p) : 
        base("Grabbed State")
    {

        }
    
    public override void StateEnter(FSMSimpleEnemyBehavior p)
    {
        

    }
    public override void StateUpdate(FSMSimpleEnemyBehavior p)
    {
        p.enemScr.transform.position = Player.grabColliderPivot.position;
        p.enemScr.transform.localRotation = Player.grabColliderPivot.rotation;
        // Se il player ha cambiato stato in THROW allora VOLA VIAAAAA
        if(Player.FSM.playerThrowState.isActive)
        {
            Debug.LogWarning("THROW STATE");
            p.SwitchState(p.simpleEnemyThrownState);
        }
        // Se non sta lanciando e non sta grabbando, sganciati
        if (!Player.FSM.playerGrabState.isActive && !Player.FSM.playerThrowState.isActive)
        {
            Debug.LogWarning("Grab state broken");
            p.SwitchState(p.simpleEnemyIdleState);
        }
    }

    public override void StateExit(FSMSimpleEnemyBehavior p)
    {

    }
}