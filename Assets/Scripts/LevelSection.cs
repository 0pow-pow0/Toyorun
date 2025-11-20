using EntitiesCostants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Le sezioni possono essere formate da simpleEn. e mageEn..
/// Se sono presenti ME. i SE. spwaneranno all'infinito finche' tutti i ME, non saranno morti.
/// Altrimenti basta che i SE muoiano tutti.
/// </summary>
public class LevelSection : MonoBehaviour
{
    // Serve per sapere se il player e' entrato nell'area della nuova sezione
    bool isActive;
    bool hasMages = false;
    int deadSimpleEnemies = 0;
    int deadMageEnemies = 0;

    [SerializeField] public GameObject player;
    [SerializeField] public SphereCollider range;
    [SerializeField] public GameObject[] walls;
    [SerializeField] public GameObject[] simpleEnemies;
    [SerializeField] public GameObject[] mageEnemies;
    [SerializeField] public ParticleSystem prtSys;


    // Inizializza la sezione mettendo i muri e spawnando i nemici xdddd
    // E comunicando all'enemymanager che questa e' il nuovo livello attivo
    void InitializeSection()
    {
        prtSys.Play();
        EnemyManager.instance().ChangeActiveLevelSection(this);
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(true);
        }

        for (int i = 0; i < simpleEnemies.Length; i++)
        {
            SimpleEnemy enemScr = simpleEnemies[i].GetComponent<SimpleEnemy>();
            simpleEnemies[i].SetActive(true);
            enemScr.delta = EnemyManager.mng.RequestEnemyDelta(enemScr.transform, enemScr.delta);
            //Debug.Log("Delta " + i + ": " + simpleEnemies[i].GetComponent<SimpleEnemy>().delta);
        }

        for (int i = 0; i < mageEnemies.Length; i++)
        {
            MageEnemy enemScr = mageEnemies[i].GetComponent<MageEnemy>();
            mageEnemies[i].SetActive(true);
            hasMages = true;
        }
    }

    void Awake()
    {
        prtSys.Stop();

        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(false);
        }

        for (int i = 0; i < simpleEnemies.Length; i++)
        {
            simpleEnemies[i].SetActive(false);
        }

        for (int i = 0; i < mageEnemies.Length; i++)
        {
            mageEnemies[i].SetActive(false);
        }
        Debug.Log("asfkansfon");
    }



    #region RESPAWN E RESET HP
    void RespawnSimpleEnemies()
    {
        for (int i = 0; i < simpleEnemies.Length; i++)
        {
            simpleEnemies[i].SetActive(true);
            SimpleEnemy enemScr = simpleEnemies[i].GetComponent<SimpleEnemy>();
            // Resetta posizione

            enemScr.agent.Warp(enemScr.spawnPos);
            //enemScr.transform.position = new Vector3(
            //    enemScr.spawnPos.x, 
            //    enemScr.spawnPos.y, 
            //    enemScr.spawnPos.z);
            // Resetta hp
            enemScr.hp = SimpleEnemyCostants.instance().START_HP;
            enemScr.isDead = false;
            enemScr.rb.velocity = Vector3.zero;
        }
    }

    void RespawnMageEnemies()
    {
        for (int i = 0; i < mageEnemies.Length; i++)
        {
            mageEnemies[i].SetActive(true);
            MageEnemy enemScr = mageEnemies[i].GetComponent<MageEnemy>();
            // Resetta posizione
            enemScr.transform.position = new Vector3(
                enemScr.spawnPos.x,
                enemScr.spawnPos.y,
                enemScr.spawnPos.z);
            // Resetta hp
            enemScr.hp = MageEnemyCostants.instance().START_HP;
        }
    }

    void ResetSimpleEnemiesHP()
    {
        for(int i = 0;i < simpleEnemies.Length;i++)
        {
            simpleEnemies[i].GetComponent<SimpleEnemy>().hp =
                SimpleEnemyCostants.instance().START_HP;
        }
    }

    void ResetMageEnemiesHP()
    {
        for(int i = 0; i < mageEnemies.Length; i++)
        {
            mageEnemies[i].GetComponent<MageEnemy>().hp =
                MageEnemyCostants.instance().START_HP;
        }
    }
    #endregion


    #region LOGICA LEVELSECTION


    /// <summary>
    /// Controlla se i nemici attivi sono morti
    /// Se si' disattivali e aggiungi 1 al counter di morti
    /// </summary>
    void CheckDeaths()
    {
        for (int i = 0; i < simpleEnemies.Length; i++)
        {
            if (simpleEnemies[i].activeSelf)
            {
                if (simpleEnemies[i].GetComponent<SimpleEnemy>().isDead)
                {
                    deadSimpleEnemies++;
                    simpleEnemies[i].SetActive(false);
                }
            }
        }

        for (int i = 0; i < mageEnemies.Length; i++)
        {
            if (mageEnemies[i].activeSelf)
            {
                if (mageEnemies[i].GetComponent<MageEnemy>().isDead)
                {
                    deadMageEnemies++;
                    mageEnemies[i].SetActive(false);
                }
            }
        }
    }

    // Un po' bruttino xd ma se avessi avuto i function pointers AVREI POTUTO FARLO
    void UpdateWithMages()
    {
        CheckDeaths();

        // Se tutti i SE sono morti
        if(deadSimpleEnemies >= simpleEnemies.Length)
        {
            // MA ci sono ancora ME
            if(deadMageEnemies < mageEnemies.Length)
            {
                // Respawna i SE
                RespawnSimpleEnemies();
                deadSimpleEnemies = 0;
            }
            // E sono morti tutti i ME
            else if(deadMageEnemies >= mageEnemies.Length)
            {
                // Sezione livello finita!  
                StartCoroutine(DestroyLevelSection(3f));
            }
        }

    }

    void UpdateWithoutMages()
    {
        CheckDeaths();

        // Se tutti i SE sono morti 
        if (deadSimpleEnemies >= simpleEnemies.Length)
        {
            StartCoroutine(DestroyLevelSection(3f));
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!isActive) { return; }

        if (hasMages)
        {
            UpdateWithMages();
        }
        else
        {
            UpdateWithoutMages();
        }

    }
    

    /// <summary>
    /// Se il plater entra nella level section:
    /// rendi attiva ed inizializza.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isActive)
        {
            isActive = true;
            InitializeSection();
        }
    }

    // Distrugge la sezione del livello dopo una quantita' di tempo definita
    IEnumerator DestroyLevelSection(float timeBeforeDestroy)
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(transform.gameObject);
    }


    #endregion
}
