using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Stessa cosa di PlayerBaseState.cs
/// </summary>
public class FSMMageEnemyBehaviour : MonoBehaviour
{
    MageEnemyBaseState _currentState;

    #region VARIABILI DI DEBUG
    bool _d_printState = false;
    [SerializeField] TextMeshProUGUI textState;
    [SerializeField] Camera mainCam;
    #endregion

    #region STATI CONCRETI
    [NonSerialized] public MageEnemyIdleState    mageEnemyIdleState;
    [NonSerialized] public MageEnemyCastState    mageEnemyCastState;
    [NonSerialized] public MageEnemyLaunchState mageEnemyLaunchState;
    [NonSerialized] public MageEnemyHurtState mageEnemyHurtState;
    #endregion

    [SerializeField] public GameObject enem;                    // Reference al gameobject che ha lo script
    [NonSerialized] public MageEnemy enemScr;             // Cosi' non devo spammare GetComponent<SimpleEnemy>
    //[NonSerialized] public LayerMask collisionLayers;

    void Start()
    {
        enemScr = enem.GetComponent<MageEnemy>();

        mageEnemyIdleState = new MageEnemyIdleState();
        mageEnemyCastState = new MageEnemyCastState();
        mageEnemyLaunchState = new MageEnemyLaunchState();
        mageEnemyHurtState = new MageEnemyHurtState();
        _currentState = mageEnemyIdleState;
        _currentState.StateEnter(this);

        // DEBUG
        if (textState != null)
        {
            _d_printState = true;
        }
    }

    void Update()
    {
        if (GameManager.isGamePaused) { return; }

        _currentState.StateUpdate(this);

        if (_d_printState)
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


    public bool SwitchState(MageEnemyBaseState newState)
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
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnCollisionEnter(Collision other)
    {
    }
}
