using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UtilityShit;
using ComboUtilities;

/// <summary>
/// All'interno di questo stato troveremo il codice che serve per attaccare ed eseguire le combo.
/// 
/// Vengono utilizzati i seguenti timer ed utilizzata la seguente logica:
/// - Tra un attacco e l'altro il giocatore PUO' premere un tasto per proseguire la combo
/// - L'attacco deve essere eseguito prima di passare al successivo attacco della combo
///     - Per sapere quando un attacco ha finito di eseguirsi utilizzo dei timer, ovvero:
///     float ATTACCO_DURATA_[numero attacco]
/// - Se non preme input per un determinato periodo di tempo allora la combo viene spezzata
///     - Per tenere traccia del tempo in cui il player puo' reagire utilizzo il seguente timer:
///     float ATTACCO_INPUT_WINDOW
///     
/// 
/// 
///  Tolerance timer funge anche da cooldown tra le combo xdddddddd
///  non doveva andare cosi' ma va bene xdddddddddddd
/// </summary>


public class PlayerAttackState : PlayerBaseState
{
    // Quando entriamo abbiamo gia' eseguito l'attacco
    int COUNTER_ATTACCHI = 0; // Serve per sapere a che punto della combo ci troviamo

    

    // TIMERS
    /*CONST*/

    // Appena si compie un'attacco bisogna ritardare l'input di qualche secondo
    // Altrimenti basterebbe spammare input tutti al primo attacco per completare le combo
    Timer toleranceTimer = new Timer(2.5f);
    Timer delayInputCombo = new Timer(0.5f);


    bool inputRicevuto = false;

    // COLLIDER_INFO


    public PlayerAttackState(FSMPlayerBehavior p) :
        base("Attack State")
    {
    }

    public override global::System.Boolean CanEnterState(FSMPlayerBehavior p)
    {
        // Se siamo a fine catena impedisci di cambiare stato
        // Player.activeCombo.indexChain si resettera' in AnyStateUpdate
        // non appena tollerancetimer sara' scaduto.
        if (Player.activeCombo.indexChain == Player.activeCombo.sequenzaAttacchi.Length - 1)
        {
            return false;
        }

        return true;
    }
    public override void StateEnter(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isAttack", true);

        // Se non e' finito
        if (!toleranceTimer.HasEnded())
        {
            // Se il toleranceTimer ancora non e' scaduto allora noi possiamo proseguire la combo 
            // a tutti gli effetti
            COUNTER_ATTACCHI++;
            Debug.Log("PROCEED WITH COMBO!");
            inputRicevuto = false;
            Player.activeCombo.indexChain++;
            //p.plrScr.anim.SetTrigger(Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].animationTrigger);
            //delayInputCombo.Restart();
            //toleranceTimer.Restart();
        }
        // Se e' finito
        else
        {
            COUNTER_ATTACCHI = 1;
        }
        p.plrScr.anim.SetTrigger(Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].animationTrigger);
        Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].collider.SetActive(true);
        toleranceTimer.Restart();
    }

    public override void StateUpdate(FSMPlayerBehavior p)
    {
        // Se il delay di input e' terminato gli input di attacco sono registrati
        // ed attivano un'interruttore che comunica che l'input e' stato inviato nel timing giusto
        if(delayInputCombo.HasEnded())
        {
            if(p.plrScr.ProcessaInputAttacco())
            {
                COUNTER_ATTACCHI++;
                inputRicevuto = true;
            }
        }


        // Se termina l'animazione di attacco permetti di cambiare stato
        // MA se e' stato premuto il tasto di input ed e' finita l'animazione allora bisogna continuare
        // Ammesso che non sia l'ultimo input della catena
        if(!inputRicevuto || Player.activeCombo.indexChain == Player.activeCombo.sequenzaAttacchi.Length-1)
        {
            #region TRANSITIONS
            if (Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].timerAnimazione.HasEnded())
            {

                p.SwitchState(p.playerIdleState);
                // TODO: CAMBIARE DIRETTAMENTE A RUNNING?
            }
            #endregion
        }
        else
        {   
            if (Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].timerAnimazione.HasEnded())
            {
                // CONTINUO COMBO


                // Resetta collider dell'attacco precedente
                Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].collider.SetActive(false);


                // Dunque, se il player ha inviato l'input nel timing giusto
                // e se l'animazione e' terminata, allora possiamo proseguire con la combo
                Player.activeCombo.indexChain++;
                // Triggera il cambio animazione
                p.plrScr.anim.SetTrigger(Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].animationTrigger);
                Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].collider.SetActive(true);

                // Resettiamo l'interruttore
                inputRicevuto = false;

                // Ed il delay tra un attacco e l'altro per far timare il colpo
                delayInputCombo.Restart();
                // E la window di tolleranza 
                toleranceTimer.Restart();
            }
        }

        Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].timerAnimazione.UpdateTime();
        delayInputCombo.UpdateTime();

        //Debug.Log("Counter attacchi: " + COUNTER_ATTACCHI);
        //Debug.Log("Tollerance timer: " + toleranceTimer.elapsedTime);
        //Debug.Log("Delay input timer: " + delayInputCombo.elapsedTime);
        //Debug.Log("Animazione timer: " + Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].timerAnimazione.elapsedTime);
        
        GameManager.delayTimer = delayInputCombo.elapsedTime;
        GameManager.animationTimer = Player.activeCombo.sequenzaAttacchi[Player.activeCombo.indexChain].timerAnimazione.elapsedTime;
    }

    public override void StateExit(FSMPlayerBehavior p)
    {
        p.plrScr.anim.SetBool("isAttack", false);
        inputRicevuto = false;

        // Disattiva IN OGNI CASO tutti i collider
        // non sappiamo a che si potrebbe triggherare il cambio di stato
        for (int i = 0; i < Player.activeCombo.sequenzaAttacchi.Length; i++)
        {
            Player.activeCombo.sequenzaAttacchi[i].collider.SetActive(false);
        }
    }

    public override void AnyStateUpdate(FSMPlayerBehavior p)
    {
        GameManager.toleranceTimer = toleranceTimer.elapsedTime;
        if(!toleranceTimer.HasEnded())
        {
            toleranceTimer.UpdateTime();
        }
        else
        {
            //Debug.Log("Inside Tolerance timer");
            // Non appena finisce il timer della tolleranza rinizia la sequenza della combo;
            Player.activeCombo.indexChain = 0;
            inputRicevuto = false;
            Player.activeCombo.ResetAnimationTimers();
        }
    }
}
