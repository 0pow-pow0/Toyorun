using EntitiesCostants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

/// <summary>
/// La differenza fra questo e "simpleEnemyApproachState" e' che in questo caso il nemico utilizza
/// un'algoritmo che lo porta nelle VICINANZE del giocatore, senza mai avvicinarvisi effettivamente
/// </summary>
public class SimpleEnemyWalkingToState : SimpleEnemyBaseState
{
    // La differenza fra i due e' che uno si trigghera' senza alcuni condizioni -forceApproach-
    // per fare in modo che non passi tutta la vita in WalkingToState se non dovesse raggiungere il delta
    //Timer forceApproach;
    Timer switchToApproachTo;

    // Tempo prima che torni all'idle state
    Timer switchToIdle;


    public SimpleEnemyWalkingToState(FSMSimpleEnemyBehavior p) :
        base("Walking To State")
    {
        //forceApproach     = new Timer(Random.Range(4f, 10f));
        switchToApproachTo = new Timer(
            Random.Range(
            SimpleEnemyCostants.instance().WALKINGTO_MIN_TIME_TO_SWITCH_APPROACH_TO,
            SimpleEnemyCostants.instance().WALKINGTO_MAX_TIME_TO_SWITCH_APPROACH_TO));

        switchToIdle = new Timer(
            Random.Range(
            SimpleEnemyCostants.instance().WALKINGTO_MIN_TIME_TO_SWITCH_IDLE,
            SimpleEnemyCostants.instance().WALKINGTO_MAX_TIME_TO_SWITCH_IDLE)); ;
    }

    public override void StateEnter(FSMSimpleEnemyBehavior p)
    {
        p.enemScr.anim.SetBool("isWalk", true);
        //Debug.Log("WALK INIT");
        if(switchToApproachTo.HasEnded())
        {
            switchToApproachTo.ChangeMaxTime(
                Random.Range(
                    SimpleEnemyCostants.instance().WALKINGTO_MIN_TIME_TO_SWITCH_APPROACH_TO,
                    SimpleEnemyCostants.instance().WALKINGTO_MAX_TIME_TO_SWITCH_APPROACH_TO));
            switchToApproachTo.Restart();
        }
        //switchToApproachTo.Restart();
        //forceApproach.Restart();
        if(switchToIdle.HasEnded())
        {
            switchToIdle.ChangeMaxTime(Random.Range(
                SimpleEnemyCostants.instance().WALKINGTO_MIN_TIME_TO_SWITCH_IDLE,
                SimpleEnemyCostants.instance().WALKINGTO_MAX_TIME_TO_SWITCH_IDLE));
            switchToIdle.Restart();
        }
        if(!p.enemScr.SetAgentDestination())
        {
            Debug.LogError("Impossibile settatre destinazione");
        }
    }

    public override void StateUpdate(FSMSimpleEnemyBehavior p)
    {

        p.enemScr.RotateToPlayer();

        // Se e' nei pressi del delta updata il suo timer per switchare alla fase di attacco
        Vector2 enemPos = new Vector2(p.enemScr.transform.position.x, p.enemScr.transform.position.z);
        float distanceFromDest = Vector2.Distance(enemPos, p.enemScr.GetAgentDestination2D());


        // Se si avvicina alla destinazione ferma l'animazione di movimento
        if (distanceFromDest <= 0f /*prendi costante da ENEMYMANAGER*/)
        {
            p.enemScr.anim.SetBool("isWalk", false);
            p.enemScr.anim.SetBool("isIdle", true);
            //p.SwitchState(p.simpleEnemyIdleState);
        }
        else if (distanceFromDest > 0f)
        {
            p.enemScr.anim.SetBool("isWalk", true);
            p.enemScr.anim.SetBool("isIdle", false);
        }
        //else if(Vector2.Distance(enemPos, p.enemScr.GetAgentDestination2D()) <= 15)
        //{
            switchToApproachTo.UpdateTime();
        //}

        if (switchToApproachTo.HasEnded() )//|| forceApproach.HasEnded())
        {
            switchToApproachTo.Restart();
            //forceApproach.Restart();
            p.SwitchState(p.simpleEnemyApproachState);
        }

        // Tempo scaduto per muoversi, fermati!
        if (switchToIdle.HasEnded())
        {
            p.SwitchState(p.simpleEnemyIdleState);
        }
        
        switchToIdle.UpdateTime();
    }

    public override void StateExit(FSMSimpleEnemyBehavior p)
    {
        p.enemScr.anim.SetBool("isIdle", false);
        p.enemScr.anim.SetBool("isWalk", false);
        p.enemScr.ResetAgentDestination();
    }


    public override void AnyStateUpdate(FSMSimpleEnemyBehavior p)
    {
        //if(!forceApproach.HasEnded())
        {
            //forceApproach.UpdateTime();
        }
        //else
        {

            //switchToApproachTo.Restart();
            //forceApproach.Restart();

            //// Evita che vada in stato di attacco mentre e' grabbato o lanciato
            //if(!p.simpleEnemyGrabbedState.isActive && !p.simpleEnemyThrownState.isActive)
            //{

            //    forceApproach.ChangeMaxTime(Random.Range(6f, 10f));
            //    Debug.Log("Changing");
            //    //p.SwitchState(p.simpleEnemyApproachState);
            //}
        }
    }
}