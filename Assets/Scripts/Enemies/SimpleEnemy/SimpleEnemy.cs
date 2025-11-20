using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using EntitiesCostants;
using UnityEditor;

/// <summary>
/// In pratica l'intelligenza sara' abbastanza semplice, AGGIUNGERE SPIEGAZIONE
/// </summary>
public class SimpleEnemy : MonoBehaviour
{
    // Serve per comunicare alla levelSection che il nemico e' morto
    [NonSerialized] public bool isDead = false;

    // Saranno utilizzati all'interno degli stati della FSM
    #region RIFERIMENTI
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Animator anim;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public GameObject plr;
    // Background della barra di caricamento dell'attacco
    // Sara' un plane, per ora e' cosi'
    // Non faro' modifiche a questo piano sara' solo attivato e disattivato 
    [SerializeField] public GameObject attackHintBackground;
    
    // Barra che si scalera' in base al percorso del caricamento
    // Anche questo e' un plane XDDDDD
    // In realta' e' un riferimento al parent che contiene il piano
    // In questo modo posso scalarlo verso la direzione giusta senza difficolta'
    [SerializeField] public GameObject attackHintSliderParent;

    [SerializeField] public BoxCollider bodyCollider;
    [SerializeField] public BoxCollider attackCollider;
    [SerializeField] public Outline outlineScr;

    #endregion

    #region VARIABILI DI GAMEPLAY
    [NonSerialized] public int hp;
    [NonSerialized] public int attack; 
    // Viene settato a runtime, prendendo il valore iniziale del transform
    [NonSerialized] public Vector3 spawnPos;    
    #endregion

    // Sarebbe la variabile che consente di evitare che i nemici si accavallino mentre
    // seguono il player, in pratica e' una sorta di "distanza" della destinazione che deve
    // raggiungere DAL player.
    // Quindi il nemico andra' verso PLAYERPOS - delta.
    [NonSerialized] public Vector2 delta; // Ci interessano solo le coordinato x e z

    /// <summary>
    /// Setta la destinatzione del NavMeshAgent alla posizione del player + delta.
    /// </summary>
    /// <param name="shouldApplyDelta">Si spostera' verso il player considerando il delta?</param>
    public bool SetAgentDestination(bool shouldApplyDelta = true)
    {
        bool result = false; 
        if(shouldApplyDelta)
        {
            result = agent.SetDestination(new Vector3(
                plr.transform.position.x + delta.x, 
                plr.transform.position.y,
                plr.transform.position.z + delta.y));
        }
        else
        {
            result = agent.SetDestination(new Vector3(
                plr.transform.position.x,               
                plr.transform.position.y,
                plr.transform.position.z));
        }

        return result;
    }

    /// <summary>
    /// Ottieni la x e la z della posizione del nemico.
    /// </summary>
    public Vector2 GetAgentDestination2D()
    {
        return new Vector2(agent.destination.x, agent.destination.z);
    }

    public void ResetAgentDestination()
    {
        agent.ResetPath();
    }


    /// <summary>
    /// Ruota il nemico verso il giocatore linearmente.
    /// </summary>
    public void RotateToPlayer()
    {
        // Direzione verso cui ruotare
        // La differenza fra due vettori genera la direzione che porta da un punto all'altro
        Vector3 dir = plr.transform.position - transform.position;
        // Avendo ottenuto la direzione verso cui ci serve ruotare, creiamo una smooth rotazione con RotateTowards
        // e poi convertiamo il vettore ottenuto da questa funzione con LookRotation che crea una rotazione basandosi
        // sulle coordinate del mondo.

        transform.localRotation =
            Quaternion.LookRotation
            (
                Vector3.RotateTowards
                (
                    transform.forward, 
                    dir, 
                    SimpleEnemyCostants.instance().ROTATION_SPEED * Time.deltaTime, 0f
                )
            );
    }

    // Start is called before the first frame update
    void Awake()
    {
        attackCollider.enabled = false;
        attackHintBackground.SetActive(false);
        attackHintSliderParent.SetActive(false);
        agent.updateRotation = false;
        hp = SimpleEnemyCostants.instance().START_HP;
        spawnPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
    }

    void TriggerDeathAnimation()
    {

    }

    void TriggerRespawnAnimation()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0) { isDead = true; }  
    }

        


    void OnDisable()
    {
        //transform.gameObject.GetComponent<FSMSimpleEnemyBehavior>().SwitchState(
        //    transform.gameObject.GetComponent<FSMSimpleEnemyBehavior>().simpleEnemyIdleState);
    }
}
