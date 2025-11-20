using System;
using TMPro;
using UnityEngine;
using EntitiesCostants;
using UnityEditor;
using UtilityShit;
using System.Collections;

public class MageEnemy : MonoBehaviour
{
    // Serve per comunicare alla levelSection che il nemico e' morto
    [NonSerialized] public bool isDead = false;

    // Saranno utilizzati all'interno degli stati della FSM
    #region RIFERIMENTI
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Animator anim;
    //[SerializeField] private NavMeshAgent agent;
    [SerializeField] public GameObject plr;
    [SerializeField] public BoxCollider bodyCollider;
    [SerializeField] public GameObject shield;
    #endregion

    #region VARIABILI DI GAMEPLAY
    [NonSerialized] public int hp;
    [NonSerialized] public int attack;
    [SerializeField] GameObject sphereAttack;
    [SerializeField] GameObject straightAttack;
    [SerializeField] GameObject waveAttack;
    [NonSerialized] public Timer shieldTimer; // Timer shield prima di disattivar
     public bool isImmortal = false;
    [NonSerialized] public Vector3 spawnPos;

    #endregion


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


    }

    ///<summary>
    /// Ritorna un attacco casuale con trigger associato.
    /// Tutto lo script del MageEnemy e' disordinato, ne sono consapevole, lol
    /// </summary>
    public (GameObject, MageEnemyAttackInformations) RandomAttack()
    {
        int r = UnityEngine.Random.Range(1,1);

        GameObject obj = null;
        MageEnemyAttackInformations infos = null;
        switch(r)
        {
            case 1:
                obj = sphereAttack;
                infos = MageEnemySphereAttack.informations;
                break;
            ///:
        }
        
        return (obj, infos);
    }


    private void Awake()
    {
        spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    // Start is called before the first frame update
    void Start()
    {
        shieldTimer = new Timer(
            UnityEngine.Random.Range(
                MageEnemyCostants.instance().SHIELD_MIN_TIME_TO_DESTROY,
                MageEnemyCostants.instance().SHIELD_MAX_TIME_TO_DESTROY
                )
            );
        hp = MageEnemyCostants.instance().START_HP;
    }
    /*
    IEnumerator EnableDisableShield()
    {
        isImmortal = false;
        shield.SetActive(false);
        yield return new WaitForSeconds(
            MageEnemyCostants.instance().SHIELD_DESTROYED_DURATION);

        shield.SetActive(true);
        isImmortal = true;
        shieldTimer.ChangeMaxTime(
            UnityEngine.Random.Range(
                MageEnemyCostants.instance().SHIELD_MIN_TIME_TO_DESTROY,
                MageEnemyCostants.instance().SHIELD_MAX_TIME_TO_DESTROY
                )
            );
        shieldTimer.Restart();
    }*/

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0) { isDead = true; }
        shield.SetActive(false);
        isImmortal = false;

    }

    private void OnDrawGizmos()
    {
       // Debug.Log("Pos: " + bodyCollider.transform.position + " Size:" + bodyCollider.size);
        //Gizmos.color = Color.yellow;
       // Gizmos.DrawCube(bodyCollider.transform.position, bodyCollider.size);
    }

}
