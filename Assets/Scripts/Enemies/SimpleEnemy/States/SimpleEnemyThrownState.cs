using EntitiesCostants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UtilityShit;

/// <summary>
/// In questo stato il nemico si avvicina finche' non e' abbastanza vicino per passare in fase di attacco
/// </summary>
public class SimpleEnemyThrownState : SimpleEnemyBaseState
{
    Vector3 posWhenThrown;

    // Evita di volare per 30 minuti se non dovesse raggiungere la destinazione
    // forse e' meglio usare questo che la distanza
    Timer flightTime;
    public SimpleEnemyThrownState(FSMSimpleEnemyBehavior p) :
        base("Thrown State")
    {
        flightTime = new Timer(
            SimpleEnemyCostants.instance().THROWN_MAX_FLIGHT_TIME);
    }

    public override void StateEnter(FSMSimpleEnemyBehavior p)
    {
        p.enemScr.anim.SetBool("isThrown", true);
        posWhenThrown = p.enemScr.transform.position;

        // Devo per forza staccarlo altrimenti non funzionano correttamente le collisioni
        // (race condition fra il rigidbody e il navmeshagent.
        p.enemScr.ResetAgentDestination();
        //p.enem.GetComponent<NavMeshAgent>().enabled = false;

        // Forza di lancio orientata in base alla direzione del player
        // Cambiando la velocita' dell'agente si resetta automaticamente il path che 
        // sta seguendo, permettendomi di manipolarla manualmente

        p.enemScr.rb.velocity = 
            // Crea rotazione tra direzione nemico e potenza forza
            Quaternion.FromToRotation(
                SimpleEnemyCostants.instance().THROWN_THROW_FORCE,
                // Poi moltiplica per ruotare potenza forza e direzionarla alla direzione del nemico
                p.enemScr.transform.forward) * 
            SimpleEnemyCostants.instance().THROWN_THROW_FORCE;
        flightTime.Restart();
    }

    public override void StateUpdate(FSMSimpleEnemyBehavior p)
    {
       if(Vector3.Distance(posWhenThrown, p.enemScr.transform.position) >= 
            SimpleEnemyCostants.instance().THROWN_MAX_TRAVEL_DISTANCE)
       {
            //p.SwitchState(p.simpleEnemyThrownState);
       }

       if(flightTime.HasEnded())
       {
            // TODO magari posso stunnarlo prima di fargli cambiare stato   
            p.SwitchState(p.simpleEnemyIdleState);
            Debug.Log("End");
       }
        Debug.Log(p.enem.GetComponent<NavMeshAgent>().enabled);

        flightTime.UpdateTime();
    }

    public override void StateExit(FSMSimpleEnemyBehavior p)
    {
        p.enem.GetComponent<NavMeshAgent>().enabled = true;
        p.enemScr.rb.velocity = Vector3.zero;
        p.enemScr.anim.SetBool("isThrown", false);
    }

    public override void AnyStateUpdate(FSMSimpleEnemyBehavior p)
    {

    }
}