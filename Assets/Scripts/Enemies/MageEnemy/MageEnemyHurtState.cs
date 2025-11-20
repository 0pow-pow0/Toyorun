using UnityEngine;
using EntitiesCostants;
using UtilityShit;


/// <summary>
/// Non si vedra' lo scudo, ma sara' immortale, come oppuranamente comunicato dall'outline xd
/// </summary>
public class MageEnemyHurtState : MageEnemyBaseState
{
    Timer timeToSwitch; 

    public MageEnemyHurtState() :
   base("Hurt State")
    {
        timeToSwitch = new Timer(
            MageEnemyCostants.instance().HURT_TIME_TO_SWITCH);
    }

    // Non puo' essere colpito
    public override void CheckCollisions(FSMMageEnemyBehaviour p)
    {
    }

    public override void StateEnter(FSMMageEnemyBehaviour p)
    {
        p.enemScr.hp--;
        p.enemScr.anim.SetBool("isHurt", true);
    }

    public override void StateExit(FSMMageEnemyBehaviour p)
    {
        p.enemScr.anim.SetBool("isHurt", false);
        //p.enemScr.shield.SetActive(true);
        //p.enemScr.isImmortal = true;
        p.enemScr.shieldTimer.ChangeMaxTime(
        UnityEngine.Random.Range(
        MageEnemyCostants.instance().SHIELD_MIN_TIME_TO_DESTROY,
        MageEnemyCostants.instance().SHIELD_MAX_TIME_TO_DESTROY
        ));
        p.enemScr.shieldTimer.Restart();
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