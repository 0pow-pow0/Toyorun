using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Stessa cosa di PlayerBaseState.cs
/// </summary>
public class FSMSimpleEnemyBehavior : MonoBehaviour
{
    SimpleEnemyBaseState _currentState;

    #region VARIABILI DI DEBUG
    bool _d_printState = false;
    [SerializeField] TextMeshProUGUI textState;
    [SerializeField] Camera mainCam;
    #endregion

    #region STATI CONCRETI
    public SimpleEnemyIdleState simpleEnemyIdleState;
    public SimpleEnemyWalkingToState simpleEnemyWalkingToState;
    public SimpleEnemyAttackState simpleEnemyAttackState;
    public SimpleEnemyApproachState simpleEnemyApproachState;
    public SimpleEnemyHurtState simpleEnemyHurtState;
    public SimpleEnemyGrabbedState simpleEnemyGrabbedState;
    public SimpleEnemyThrownState simpleEnemyThrownState; 
    #endregion

    [SerializeField] public GameObject enem;                    // Reference al gameobject che ha lo script
    [NonSerialized] public SimpleEnemy enemScr;             // Cosi' non devo spammare GetComponent<SimpleEnemy>
    //[NonSerialized] public LayerMask collisionLayers;

    void Start()
    {
        enemScr = enem.GetComponent<SimpleEnemy>();
        simpleEnemyIdleState        = new SimpleEnemyIdleState(this);
        simpleEnemyWalkingToState = new SimpleEnemyWalkingToState(this);
        simpleEnemyApproachState = new SimpleEnemyApproachState(this);
        simpleEnemyAttackState = new SimpleEnemyAttackState(this);
        simpleEnemyHurtState = new SimpleEnemyHurtState(this);
        simpleEnemyGrabbedState = new SimpleEnemyGrabbedState(this);
        simpleEnemyThrownState = new SimpleEnemyThrownState(this);
        _currentState = simpleEnemyWalkingToState;
        _currentState.StateEnter(this);


        // DEBUG
        if(textState != null)
        {
            _d_printState = true;
        }
    }

    void Update()
    {
        if (GameManager.isGamePaused) { return; }
        simpleEnemyWalkingToState.AnyStateUpdate(this);
        simpleEnemyAttackState.AnyStateUpdate(this);
        simpleEnemyIdleState.AnyStateUpdate(this);
        simpleEnemyApproachState.AnyStateUpdate(this);
        simpleEnemyHurtState.AnyStateUpdate(this);
        simpleEnemyGrabbedState.AnyStateUpdate(this);
        simpleEnemyThrownState.AnyStateUpdate(this);
        _currentState.StateUpdate(this);
        
        if(_d_printState)
        {
            Vector3 enemPosToScreen = mainCam.WorldToScreenPoint(enemScr.transform.position);
            textState.transform.position = new Vector3(
                enemPosToScreen.x,
                enemPosToScreen.y,
                0);

            textState.text = _currentState._d_stateName;
        }
    }

    private void FixedUpdate()
    {
        _currentState.CheckCollisions(this);
    }


    public bool SwitchState(SimpleEnemyBaseState newState)
    {
        if (newState.CanEnterState(this))
        {
            _currentState.StateExit(this);
            _currentState.isActive = false;
            

            _currentState = newState;
            _currentState.StateEnter(this);
            _currentState.isActive = true;
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.layer == LayerMask.NameToLayer("PlayerAttackCollider"))
        //{
            //Debug.Log("COLLIS");
        //}
        //_currentState.OnTriggerEnter(this, other);
    }

    private void OnTriggerStay(Collider other)
    {
        //_currentState.OnTriggerStay(this, other);
    }

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("askdaskdka");
    }
}
