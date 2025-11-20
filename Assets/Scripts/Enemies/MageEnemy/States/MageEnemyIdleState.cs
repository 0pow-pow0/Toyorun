using UnityEngine;
using EntitiesCostants;
using UtilityShit;

public class MageEnemyIdleState : MageEnemyBaseState
{
    public Timer timeToSwitch; // Tempo prima di cambiare a cast state
 
     public MageEnemyIdleState() : 
    base("Idle State")
    {
        timeToSwitch = new Timer(1f
            //Random.Range(
            //MageEnemyCostants.instance().IDLE_MIN_TIME_TO_SWITCH,
            //MageEnemyCostants.instance().IDLE_MAX_TIME_TO_SWITCH)
            );
    }



    public override void StateEnter(FSMMageEnemyBehaviour p)
    {


        //timeToSwitch.ChangeMaxTime(
        //    Random.Range(
        //        MageEnemyCostants.instance().IDLE_MIN_TIME_TO_SWITCH,
        //        MageEnemyCostants.instance().IDLE_MAX_TIME_TO_SWITCH
        //        )
        //    );

        timeToSwitch.Restart();
    }

    public override void StateExit(FSMMageEnemyBehaviour p)
    {
    }

    public override void StateUpdate(FSMMageEnemyBehaviour p)
    {
        if(timeToSwitch.HasEnded())
        {
            p.SwitchState(p.mageEnemyCastState);
        }

        p.enemScr.RotateToPlayer();

        timeToSwitch.UpdateTime();
    }
}