using UnityEngine;
using UtilityShit;
using EntitiesCostants;
using System.Collections;

///<summary>
/// Ogni attacco avra' un suo prefab [PrefabAtt]
/// Ogni prefab utilizzera' dei proiettili, a loro volta dei prefab. [ProjAtt]
/// Ogni PrefabAtt prendera' le componenti dai ProjAtt in base alla loro struttura ad albero,
/// non sapevo come fare altrimenti. 
/// Dopo di che' utilizzera i proiettili a suo piacimento, per creare tutte le combinazioni che voglio.
/// 
/// DEVE ESSERE ATTIVO NELL'EDITOR SE NO NON INSTANZIA UNA CIOLLA.
/// </summary>


/// <summary>
/// Questo attacco crea la bellezza di 3 sfere che compaiono e scompaiono
/// In pratica funziona che da un prefab proiettile prendiamo i componenti necessari basandosi sulla
/// sua struttura ad albero. Cio' significa che modificando la struttura ad albero del prefab esplode tutto.
/// Dopo aver preso i componenti necessari li utilizziamo nella logica dell'attacco.
/// </summary>
public class MageEnemySphereAttack : MonoBehaviour
{
    [SerializeField] GameObject plr;

    public static MageEnemyAttackInformations informations; 
    public MageEnemyProjectile<SphereCollider>[] projectiles;

    // Centro in cui viene generato l'attacco
    public Vector3 creationPoint { get; private set; }
    Timer castTime; // Tempo prima che i collider si attivino
    bool hasBeenCasted = false; // Interruttore per evitare che la coroutine venga spammata

    public MageEnemySphereAttack()
    {
        informations = new MageEnemyAttackInformations(
            MageEnemyCostants.instance().SPHERE_ATTACK_CAST_DURATION,
            MageEnemyCostants.instance().SPHERE_ATTACK_CAST_DURATION,
            MageEnemyCostants.instance().SPHERE_ATTACK_DAMAGE,
            MageEnemyCostants.instance().SPHERE_ATTACK_ANIM_TRIGGER
            );

        castTime = new Timer(
            MageEnemyCostants.instance().SPHERE_ATTACK_CAST_DURATION);
    }

    public void SetAttachedObject(bool b)
    {
        gameObject.SetActive(b);
    }

    private void Awake()
    {
        // In pratica csharp alloca solo lo spazio per 3 classi ma non crea 3 oggetti separati...
        projectiles = new MageEnemyProjectile<SphereCollider>[3];

        // ...quindi li creo manualmente
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i] = new MageEnemyProjectile<SphereCollider>();
        }

        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].parent = transform.GetChild(i).gameObject;
            projectiles[i].background = projectiles[i].parent.transform.GetChild(0).gameObject;
            projectiles[i].hintSlider = projectiles[i].background.transform.GetChild(0).gameObject;
            projectiles[i].collider = 
                projectiles[i].background.transform.GetChild(1).GetComponent<SphereCollider>();

            // Inizializza i collider a false, cosi' al primo cast saranno gia' disattivati
            projectiles[i].collider.enabled = false;
            if(projectiles[i].parent == null ||
                projectiles[i].background == null ||
                projectiles[i].hintSlider == null ||
                projectiles[i].collider == null)
            {
                Debug.LogError(projectiles[i].parent + " " +
                    projectiles[i].collider + " " +
                    projectiles[i].hintSlider + " " +
                    projectiles[i].background);
            }
        }

        Debug.Log("Created!");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Setta la posizione delle sfere in base al vettore passato, 
    /// considerando anche i vettori posizione MageEnemyCostants che 
    /// servono per creare il pattern desiderato
    /// </summary>
    /// <param name="newCreationPoint"></param>
    public void SetCreationPoint(Vector3 newCreationPoint)
    {
        creationPoint = newCreationPoint;

        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].parent.transform.position = creationPoint +
                MageEnemyCostants.instance().SPHERE_ATTACK_POSITIONS[i];
        }
    }
    
    
    private void OnEnable()
    {
        // Resetta tutto
        // Tranne durationTime che verra' resettata non appena casttime sara' scaduto
        castTime.Restart();
        SetCreationPoint(plr.transform.position);
    }

    /// <summary>
    /// Attiva i cast e poi disattivali dopo un tot di secondi
    /// </summary>
    IEnumerator EnableAndDisableCastOnTimer()
    {
        // Resettata in OnDisable()
        hasBeenCasted = true;
        for(int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].collider.enabled = true; 
        }
        yield return new WaitForSeconds(MageEnemyCostants.instance().SPHERE_ATTACK_MAGIC_DURATION);
        for (int i = 0; i < projectiles.Length; i++)
        {

            projectiles[i].collider.enabled = false;
        }
    }


    private void Update()
    {
        if(!castTime.HasEnded())
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                // Espandi l'hint slider finche' non diventa della stessa grandezza del background
                // (di cui e' figlio)
                float t = castTime.elapsedTime / castTime.maxTime;
                projectiles[i].hintSlider.transform.localScale = new Vector3(
                        MageEnemyCostants.instance().SPHERE_ATTACK_HINT_SLIDER_MAX_DIMENSIONS.x * t,
                        projectiles[i].hintSlider.transform.localScale.y,
                        MageEnemyCostants.instance().SPHERE_ATTACK_HINT_SLIDER_MAX_DIMENSIONS.z * t);

            }
            castTime.UpdateTime();
        }
        // Appena il tempo di cast e' terminato
        else 
        {
            if(!hasBeenCasted)
            {
                StartCoroutine(EnableAndDisableCastOnTimer());
            }
        }
    }

    private void OnDisable()
    {
        for(int i = 0; i < projectiles.Length; i++)
        {
            // Resetta valori di hintSlider a zero per nascondere
            projectiles[i].hintSlider.transform.localScale = new Vector3(
                0,
                projectiles[i].hintSlider.transform.localScale.y,
                0);
        }

        // La resetto qui per evitare problemi di flow
        // ed evitare che si ritriggherino la coroutine dei collider
        hasBeenCasted = false;
    }

}