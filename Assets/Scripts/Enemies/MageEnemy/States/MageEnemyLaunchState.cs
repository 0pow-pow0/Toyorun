using UnityEngine;
using EntitiesCostants;
using UtilityShit;

public class MageEnemyLaunchState : MageEnemyBaseState
{
    Timer timeToSwitch; // Tempo prima di cambiare stato
    public GameObject attackToActivate;
    public MageEnemyAttackInformations attackInfos;

    public MageEnemyLaunchState() :
   base("Launch State")
    {
        timeToSwitch = new Timer(
            MageEnemyCostants.instance().LAUNCH_TIME_TO_SWITCH);
    }


    public override void StateEnter(FSMMageEnemyBehaviour p)
    {
        p.enemScr.anim.SetBool("isLaunch", true);
        timeToSwitch.ChangeMaxTime(attackInfos.DURATION_TIME);
        timeToSwitch.Restart();
        Debug.Log("Launch stae enter");
    }

    public override void StateExit(FSMMageEnemyBehaviour p)
    {
        p.enemScr.anim.SetBool("isLaunch", false);
        attackToActivate.SetActive(false);
    }

    public override void StateUpdate(FSMMageEnemyBehaviour p)
    {
        if(timeToSwitch.HasEnded())
        {
            p.SwitchState(p.mageEnemyIdleState);
        }

        timeToSwitch.UpdateTime();
    }
}