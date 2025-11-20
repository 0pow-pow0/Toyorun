using UnityEngine;
using EntitiesCostants;
using UtilityShit;

public class MageEnemyCastState : MageEnemyBaseState
{
    Timer timeToSwitch; // Tempo prima di cambiare stato
    GameObject activeAttack;
    MageEnemyAttackInformations attackInfos;

    public MageEnemyCastState() :
   base("Cast State")
    {
        timeToSwitch = new Timer(0f);
    }

    public override void StateEnter(FSMMageEnemyBehaviour p)
    {
        (activeAttack, attackInfos) = p.enemScr.RandomAttack();
        
        activeAttack.SetActive(true);
        timeToSwitch.ChangeMaxTime(attackInfos.CAST_TIME);
        timeToSwitch.Restart();
        //Debug.Log(activeAttack + " " + attackInfos.ANIMATION_TRIGGER + " " + 
            //  attackInfos.CAST_TIME);

        p.enemScr.anim.SetBool("isCast", true);
        p.enemScr.anim.SetTrigger(attackInfos.ANIMATION_TRIGGER);

    }

    public override void StateExit(FSMMageEnemyBehaviour p)
    {
        p.enemScr.anim.SetBool("isCast", false);
    }

    public override void StateUpdate(FSMMageEnemyBehaviour p)
    {
        if(timeToSwitch.HasEnded())
        {
            p.mageEnemyLaunchState.attackToActivate = activeAttack;
            p.mageEnemyLaunchState.attackInfos = attackInfos;
            p.SwitchState(p.mageEnemyLaunchState);
        }

        p.enemScr.RotateToPlayer();

        timeToSwitch.UpdateTime();
    }
}