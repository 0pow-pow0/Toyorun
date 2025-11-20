using ComboUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;
using UnityEditor;
using UnityEngine.SceneManagement;
using EntitiesCostants;

// Player e' utilizzato come classe di base dagli stati del player
// dalla quale attingono per tutte le informazioni necessarie
// dunque molti frammenti di codice si trovano nel codice degli stati 


// METTERE SOLO AL PLAYER
public class Player : MonoBehaviour
{
    /// <summary>
    /// Ho deciso di raggruppare molte variabili utilizzate dagli altri stati
    /// in questo file, per evitare la dispersione di 200mila variabili
    /// Soprattutto quelle piu' probabili che vengano cambiate
    /// </summary>
    #region RIFERIMENTI
    [SerializeField] public Outline outlineScr;
    [SerializeField] public Animator anim;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public BoxCollider bodyCollider;
    [SerializeField] public BoxCollider grabCollider;
    [SerializeField] static public Transform grabColliderPivot;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private StyleBar styleBar;

    // Riferimento alla sua stessa FSM
    [NonSerialized] public static FSMPlayerBehavior FSM;
    #endregion

    #region VARIABILI DEL GAMEPLAY
    // Li metto private cosi' non possono essere modificate a valori non validi
    private static int maxHp = 2;
    private static int hp = 2;

    public static bool isDead = false;
    public bool isInvincible = false;
    #endregion

    #region VARIABILI DELLA FISICA
    [SerializeField] public float speed;
    #endregion

    #region COSTANTI
    // QUESTO e' PER FARMI RICHIAMARE DA TE FIAMMA:
    // PENE 

    const int VELOCITA_STERZATA = 500; // Velocita' con cui gira verso la direzione data in input
    #endregion

    #region TIMER/COUNTERS
    #endregion

    // Istanziate in Awake :)
    #region COMBO LET'S FUCKING GOOOOOOOO
    Combo baseCombo;
    public static Combo activeCombo; // Per ora non serve a niente, ma col tempo MUAHAHAHAHAH, COL TEMPO CAMBIERA' TUTTO
    #endregion

    /// <summary>
    /// </summary>

    // Non instaziabile
    private Player()
    {

    }

    void Awake()
    {
        FSM = gameObject.GetComponent<FSMPlayerBehavior>();
        grabCollider.enabled = false;

        grabColliderPivot = GameObject.FindWithTag("GrabColliderPivot").transform;

        // ---------------------------- ISTANZIAMO LE COMBO LET'S FUCKING GOOOOOOOOOOOOOOOOOOOOOOOOOO
        // TODO METTI VALORI IN ENTITIES COSTANTS
        baseCombo = new Combo(3, 2.5f);
        GameObject[] arr = GameObject.FindGameObjectsWithTag("BaseCombo");
        baseCombo.sequenzaAttacchi[0] = new Attacco(new Timer(0.58f), 1, "baseComboTrigger_0", arr[0]);
        baseCombo.sequenzaAttacchi[1] = new Attacco(new Timer(0.75f), 1, "baseComboTrigger_1", arr[1]);
        baseCombo.sequenzaAttacchi[2] = new Attacco(new Timer(0.83f), 2, "baseComboTrigger_2", arr[2]);

        activeCombo = baseCombo;

        maxHp = PlayerCostants.instance().START_MAX_HP;
        hp = PlayerCostants.instance().START_HP;
        Debug.Log("Processo creazione attacchi: " + baseCombo);
    }

    void Update()
    {
        if(GameManager.isGamePaused) { return; }
        if(hp <= 0) {  isDead = true; SceneManager.LoadScene("GameOver"); }


        //Debug.Log("Has grabbed Someone: " + FSM.playerGrabState.hasGrabbedSomeone);

        //    if (Input.GetKeyDown(KeyCode.U))
        //    {
        //        GainHp();
        //    }
        if (Input.GetKeyDown(KeyCode.I))
        {
            LoseHp();
        }

        //    if (Input.GetKeyDown(KeyCode.J))
        //    {
        //        AddHp();
        //    }

        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        RemoveHp();
        //    }
        //    Debug.Log("Hp: " + hp + " MaxHp: " + maxHp);
    }

    #region GESTIONE VITA
    public static int GetMaxHp()
    {
        return maxHp;
    }

    public static int GetCurrentHp()
    {
        return hp;
    }

    public void LoseHp()
    {
        if(hp <= 0) { return; }
        healthBar.LoseHeart(hp-1);
        hp--;
    }

    /// <summary>
    /// Non puo' superare maxHp
    /// </summary>
    public void GainHp()
    {
        if (hp + 1 > maxHp) { return; }
        hp++;
        healthBar.GainHeart(hp-1);
    }

    /// <summary>
    /// Aggiunge un hp agli hp massimi, comunicandolo all'hud
    /// e triggerando la rispettiva animazione.
    /// Cura tutti gli hp :)
    /// </summary>
    public void AddHp()
    {
        maxHp++;
        healthBar.AddHeart();

        // Cura tutti i cuori
        if(hp < maxHp)
        {
            int cond = maxHp - hp;
            for(int i = 0; i < cond; i++)
            {
                GainHp();
            }
            // Si settera' da solo grazie a GainHp
            //hp = maxHp;
        } 
    }



    /// <summary>
    /// Rimuove un hp agli hp massimi, comunicandolo all'hud
    /// e triggerando la rispettiva animazione.
    /// Non si puo' scendere sotto lo zero.
    /// </summary>
    public void RemoveHp()
    {
        if(maxHp - 1 <= 0) { return; }
        
        maxHp--;
        if(hp > maxHp) { hp = maxHp; }
        StartCoroutine(healthBar.RemoveHeart(maxHp));
    }

    /// <summary>
    /// Cura i cuori feriti.
    /// </summary>
    public void GainAllHp()
    {

    }

    static void ChangeMaxHp(int newMaxHp)
    {
        if(newMaxHp == maxHp) { Debug.LogWarning("newMaxHp e maxHp sono uguali!"); return; }
            
        // Se i newMaxHp sono minori significa che ha perso cuori
        if(newMaxHp < maxHp)
        {

        }
        else if(newMaxHp > maxHp)
        {

        }
    }

    #endregion

    #region FUNZIONI GESTIONE INPUT
    // TUTTE LE FUNZIONI RITORNARE FALSE SE IL GIOCO E' IN PAUSA
    // O Time.timeScale == 0f
    // TODO: forse e' meglio usare una variabile globale in gamemanager per
    // capire se il gioco e' in pausa?


    // Sara' usata da tutti gli stati per sapere se sta attaccando
    // Siccome sara' ripetuto un paio di volte nel codice
    // Lo metto qui cosi' non devo cambiare duecentomila cose in caso di modifiche
    public bool ProcessaInputAttacco()
    {
        return Input.GetKeyDown(KeyCode.J) && Time.timeScale != 0f;
    }
    
    public bool ProcessaInputSchivata()
    {
        return Input.GetKeyDown(KeyCode.K) && Time.timeScale != 0f;
    }
    /// <summary>
    /// Uguale a grab per ora
    /// </summary>
    public bool ProcessaInputLancio()
    {
        return Input.GetKeyDown(KeyCode.L) && Time.timeScale != 0f;
    }

    public bool ProcessaInputGrab()
    {
        return Input.GetKeyDown(KeyCode.L) && Time.timeScale != 0f;
    }

    public bool ProcessaInputMuoviOggetto()
    {
        return Input.GetKeyDown(KeyCode.U) && Time.timeScale != 0f;
    }

    public bool ProcessaInputPausa()
    {
        return false;
    }

    #endregion

    public void AggiustaRotazione(float x, float z)
    {
        // Per evitare che ruoti quando non sono dati input
        if (x == 0 && z == 0) { return; }

        
        Vector3 movementRotation = new Vector3(x, 0, z);

        //Vector3 rotationToApply = Quaternion.LookRotation(movementRotation).eulerAngles; 
        Quaternion rotationToApply = Quaternion.LookRotation(movementRotation);

        //Quaternion.FromToRotation(movementRotation, Vector3.forward).eulerAngles;

        //Debug.Log("Rotation: " + rotationToApply.eulerAngles);

        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, 
            rotationToApply, VELOCITA_STERZATA*Time.deltaTime);
        // Scarta le altre dimensioni per tenere solo la rotazione sul piano Y
        //transform.localEulerAngles = new Vector3(0, rotationToApply.y);
    }
}
