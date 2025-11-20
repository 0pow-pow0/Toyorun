using EntitiesCostants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

/// <summary>
/// In questo stato il nemico si avvicina finche' non e' abbastanza vicino per passare in fase di attacco
/// </summary>
public class SimpleEnemyApproachState : SimpleEnemyBaseState
{
    Timer updateDestTime; // Ogni quanto bisogna aggiornare la posizione?

    public SimpleEnemyApproachState(FSMSimpleEnemyBehavior p) :
        base("Approach State")
    {
        updateDestTime = new Timer(0.5f);
    }

    public override void StateEnter(FSMSimpleEnemyBehavior p)
    {
        p.enemScr.anim.SetBool("isWalk", true);
        p.enemScr.SetAgentDestination(false);
        updateDestTime.Restart();
        //Debug.Log("APPROACH INIT");
    }

    public override void StateUpdate(FSMSimpleEnemyBehavior p)
    {
        // Consideriamo le posizioni come fossero bidimensionali vabb l'ho fatto gia' 3mila volte
        Vector2 plrPos = new Vector2(p.enemScr.plr.transform.position.x, p.enemScr.plr.transform.position.z);
        Vector2 enemPos = new Vector2(p.enemScr.transform.position.x, p.enemScr.transform.position.z);

        if (updateDestTime.HasEnded())
        {
            if (plrPos != p.enemScr.GetAgentDestination2D())
            {
                //Debug.Log("Setting destination!");
                p.enemScr.SetAgentDestination(false);
                updateDestTime.Restart();
            }
        }
        else
        {
            //Debug.Log("Updating Time!");
            //Debug.Log("Time: " + updateDestTime.elapsedTime + " maxtime: " + updateDestTime.maxTime);
            updateDestTime.UpdateTime();
        }


        p.enemScr.RotateToPlayer();

        // Appena si avvicina, fagli il culo
        if(Vector2.Distance(plrPos, enemPos) < 
            SimpleEnemyCostants.instance().APPROACHTO_MIN_DISTANCE_TO_ATTACK)
        {
            p.SwitchState(p.simpleEnemyAttackState);
        }
    }
    //public override void CheckCollisions(FSMSimpleEnemyBehavior p)
    //{
        // Non puo' essere scopato
        //CheckTriggerWithPlayerGrab(p);
    //}
    public override void StateExit(FSMSimpleEnemyBehavior p)
    {
        p.enemScr.ResetAgentDestination();
        p.enemScr.anim.SetBool("isWalk", false);
    }

    public override void AnyStateUpdate(FSMSimpleEnemyBehavior p)
    {

    }
}