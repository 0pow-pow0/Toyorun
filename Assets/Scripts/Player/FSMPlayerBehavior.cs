using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Finite state machine per gestire gli stati di qualunque entita'
/// E' necessario solo dare una reference del GameObject da cui si vogliono
/// ricevere informazioni utili per processare azioni nei codici dei singoli stati.
/// Ogni entita' puo' accedere alla FSM del player visto che e' staticamente referenziata in Player.cs.
/// </summary>
public class FSMPlayerBehavior : MonoBehaviour
{
    PlayerBaseState _currentState;

    [SerializeField] public GameObject plr; // Reference al GameObject del player
    [NonSerialized] public Player plrScr;  // Cosi' non devo richiamare 200 volte GetComponent<Player>()

    #region STATI CONCRETI
    const int NUMBER_OF_STATES = 3;
    public PlayerIdleState playerIdleState;
    public PlayerRunState playerRunState;
    public PlayerAttackState playerAttackState;
    public PlayerDodgeState playerDodgeState;
    public PlayerHurtState playerHurtState;
    public PlayerGrabState playerGrabState;
    public PlayerThrowState playerThrowState;
    public PlayerStunnedState playerStunnedState;
    public PlayerMoveObjectState playerMoveObjectState;
    #endregion
    
    // Non mi fa creare un array di puntatori e quindi non posso iterare devo hard coddare
    //PlayerBaseState*[] states_Iterator;

    void Start()
    {
        plrScr = GetComponent<Player>();

        playerIdleState = new PlayerIdleState();
        playerRunState = new PlayerRunState();
        playerAttackState = new PlayerAttackState(this);
        playerDodgeState = new PlayerDodgeState();
        playerHurtState = new PlayerHurtState();
        playerGrabState = new PlayerGrabState(this);
        playerThrowState = new PlayerThrowState(this);
        playerStunnedState = new PlayerStunnedState(this);
        playerMoveObjectState = new PlayerMoveObjectState(this);

        _currentState = playerIdleState;
        Debug.Log(playerIdleState);
        _currentState.StateEnter(this);
    }

    void Update()
    {
        if (GameManager.isGamePaused) { return; }
        if(Player.isDead) { return; }
        // Logica dell'any state dell'animator in pratica
        // Non posso usare gli iteratori
        // Non ha senso che ci siano questi due tizi qui, tanto so vuoti
        // ma partivo con l'idea che potessi usare gli iteratori
        // Hard coddato come piace a mia madre
        playerIdleState.AnyStateUpdate(this);   
        playerRunState.AnyStateUpdate(this);
        playerAttackState.AnyStateUpdate(this);
        playerDodgeState.AnyStateUpdate(this);
        playerHurtState.AnyStateUpdate(this);
        playerGrabState.AnyStateUpdate(this);
        playerThrowState.AnyStateUpdate(this);
        playerStunnedState.AnyStateUpdate(this);
        playerMoveObjectState.AnyStateUpdate(this);


        //Debug.Log("state: " + _currentState);
        _currentState.StateUpdate(this);
    }

    private void FixedUpdate()
    {
        _currentState.ProcessTriggers(this);
    }

    /// <summary>
    /// Lo switch state sara' possibile solo ed esclusivamente se il nuovo stato ritorna
    /// un valore true durante l'esecuzione dello StateEnter, se e' false semplicemente
    /// non si triggera ne' lo StateExit del vecchio stato ed e' come se non avessimo fatto alcuno switch.
    /// </summary>
    public bool SwitchState(PlayerBaseState newState)
    {
        if(newState.CanEnterState(this))
        {
            _currentState.StateExit(this);
            _currentState.isActive = false;

            _currentState = newState;
            _currentState.StateEnter(this); 
            _currentState.isActive = true;
            GameManager.animState = _currentState._d_stateName;
        }

        return false;
    }

    public void OnTriggerEnter(Collider other)
    {
        //_currentState.OnTriggerEnter(this, other);
    }

}
